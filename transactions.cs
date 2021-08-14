using System;
using System.Diagnostics;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   [Serializable]
   public class transactions
    {
      private Image<Gray, Single>[] _picture_1;
      private Image<Gray, Single> _picture_i_2;
      private Matrix<float>[] _picture_i_3;
      private string[] _labels;
      private double _picture_i_4;

     
      public Image<Gray, Single>[] resim_e
      {
         get { return _picture_1; }
         set { _picture_1 = value; }
      }
      public String[] labels
      {
         get { return _labels; }
         set { _labels = value; }
      }
      public double picture_p
      {
         get { return _picture_i_4; }
         set { _picture_i_4 = value; }
      }

      
      public Image<Gray, Single> picture_p_2
      {
         get { return _picture_i_2; }
         set { _picture_i_2 = value; }
      }

    
      public Matrix<float>[] picture_value
      {
         get { return _picture_i_3; }
         set { _picture_i_3 = value; }
      }

      private transactions()
      {

      }
      public transactions(Image<Gray, Byte>[] images, ref MCvTermCriteria termCrit)
         : this(images, Generateetiket(images.Length), ref termCrit)
      {
      }

      private static String[] Generateetiket(int size)
      {
         String[] labels = new string[size];
         for (int i = 0; i < size; i++)
            labels[i] = i.ToString();
         return labels;
      }
      public transactions(Image<Gray, Byte>[] images, String[] labels, ref MCvTermCriteria termCrit)
         : this(images, labels, 0, ref termCrit)
      {
      }
      public transactions(Image<Gray, Byte>[] images, String[] labels, double picture_p, ref MCvTermCriteria termCrit)
      {
         Debug.Assert(images.Length == labels.Length, "The number of images should equals the number of labels");
         Debug.Assert(picture_p >= 0.0, "Eigen-distance threshold should always >= 0.0");

         CalcEigenObjects(images, ref termCrit, out _picture_1, out _picture_i_2);


         _picture_i_3 = Array.ConvertAll<Image<Gray, Byte>, Matrix<float>>(images,
             delegate(Image<Gray, Byte> img)
             {
                return new Matrix<float>(EigenDecomposite(img, _picture_1, _picture_i_2));
             });

         _labels = labels;

         _picture_i_4 = picture_p;
      }
      public static void CalcEigenObjects(Image<Gray, Byte>[] trainingImages, ref MCvTermCriteria termCrit, out Image<Gray, Single>[] resim_e, out Image<Gray, Single> avg)
      {
         int width = trainingImages[0].Width;
         int height = trainingImages[0].Height;

         IntPtr[] inObjs = Array.ConvertAll<Image<Gray, Byte>, IntPtr>(trainingImages, delegate(Image<Gray, Byte> img) { return img.Ptr; });

         if (termCrit.max_iter <= 0 || termCrit.max_iter > trainingImages.Length)
            termCrit.max_iter = trainingImages.Length;
         
         int maxEigenObjs = termCrit.max_iter;

         #region initialize eigen images
         resim_e = new Image<Gray, float>[maxEigenObjs];
         for (int i = 0; i < resim_e.Length; i++)
            resim_e[i] = new Image<Gray, float>(width, height);
         IntPtr[] eigObjs = Array.ConvertAll<Image<Gray, Single>, IntPtr>(resim_e, delegate(Image<Gray, Single> img) { return img.Ptr; });
         #endregion

         avg = new Image<Gray, Single>(width, height);

         CvInvoke.cvCalcEigenObjects(
             inObjs,
             ref termCrit,
             eigObjs,
             null,
             avg.Ptr);
      }
      public static float[] EigenDecomposite(Image<Gray, Byte> src, Image<Gray, Single>[] resim_e, Image<Gray, Single> avg)
      {
         return CvInvoke.cvEigenDecomposite(
             src.Ptr,
             Array.ConvertAll<Image<Gray, Single>, IntPtr>(resim_e, delegate(Image<Gray, Single> img) { return img.Ptr; }),
             avg.Ptr);
      }
      
      public Image<Gray, Byte> EigenProjection(float[] eigenValue)
      {
         Image<Gray, Byte> res = new Image<Gray, byte>(_picture_i_2.Width, _picture_i_2.Height);
         CvInvoke.cvEigenProjection(
             Array.ConvertAll<Image<Gray, Single>, IntPtr>(_picture_1, delegate(Image<Gray, Single> img) { return img.Ptr; }),
             eigenValue,
             _picture_i_2.Ptr,
             res.Ptr);
         return res;
      }
      public float[] GetEigenDistances(Image<Gray, Byte> image)
      {
         using (Matrix<float> eigenValue = new Matrix<float>(EigenDecomposite(image, _picture_1, _picture_i_2)))
            return Array.ConvertAll<Matrix<float>, float>(_picture_i_3,
                delegate(Matrix<float> eigenValueI)
                {
                   return (float)CvInvoke.cvNorm(eigenValue.Ptr, eigenValueI.Ptr, Emgu.CV.CvEnum.NORM_TYPE.CV_L2, IntPtr.Zero);
                });
      }
      public void FindMostSimilarObject(Image<Gray, Byte> image, out int index, out float eigenDistance, out String label)
      {
         float[] dist = GetEigenDistances(image);

         index = 0;
         eigenDistance = dist[0];
         for (int i = 1; i < dist.Length; i++)
         {
            if (dist[i] < eigenDistance)
            {
               index = i;
               eigenDistance = dist[i];
            }
         }
         label = labels[index];
      }

     
      public String Recognize(Image<Gray, Byte> image)
      {
         int index;
         float eigenDistance;
         String label;
         FindMostSimilarObject(image, out index, out eigenDistance, out label);

         return (_picture_i_4 <= 0 || eigenDistance < _picture_i_4 )  ? _labels[index] : String.Empty;
      }
   }
}
