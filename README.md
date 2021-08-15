Yüz Algılama ve Tanıma ile Kullanıcı girişi sağlama
_____________________________________________________________________________________________________

Yaptığım proje video kaynaklarından alınan resmin bilgisayarınıza kaydedildikten sonra bilgisayarınıza kaydettiğiniz resimler içerisinden eğer eşleşen herhangi bir resim var ise tanıma sağlanarak resime verdiğiniz kullanıcı ismi ekrana gelir ve program sonlanır. 

Projem iki bölümden oluşuyor.Yüz Algılama(Face Detection) ve Yüz Tanıma(Face Recognition).

İlk önce AForce , EmguCV ve OpenCV kütüphanelerini projeye dahil etmek gerekmektedir.Ve haarcascade_frontalface_default.xml dosyasını projemize eklemeliyiz.

EmguCV bir OpenCV wrapper'idir.Yani OpenCV Framework' ünün .Net dilleri üzerinde oluşturulmuş bir kütüphanesidir.

Birinci kısımda AForge.Video.DirectShow kütüphanesi kullanılarak bigisayarınızdaki video kaynaklarını algılar.Bunlara örnek verirsek;usb web kameralar, görüntü yakalama aygıtları, video dosyaları örneklendirilebilir.Ve video kaynağını combobox'a eleman olarak ekler eğer bilgisayarınızda herhangi bir video kaynağı yok ise bir sonraki adım yüz algılama ve tanımaya geçemezsiniz.

AForge.NET kütüphanesi yine açık kaynaklı bir C# frameworküdür.Program geliştiriciler, bilimsel araştırma yapan kişiler için özellikle görüntüleme işleme de ve yapay zeka uygulamalarında kullanılması amacı ile geliştirilmiş bir kütüphanedir.Sadece bu iki alanda değil tabi, nöral ağlarda yani insan beyni ve sinir sistemini taklit eden yapay zeka uygulamalarında, genetik algoritmasında, fuzzy lojikte yani bulanık mantıkta, machine learning dediğimiz makine öğrenmesinde ve robotikte de çok kullanılır.

İkinci adımda bilgisayarınızda kayıtlı bir yüz var ise devam edilir eğer yok ise mecburen kaydetmelisiniz.Resimler kaydedilirken kaydetme esnasında verdiğiniz isim ile siyah-beyaz
olarak .jpeg formatında System.IO kullanılarak keydedilmektedir.Timer kullanılarak eğer tanıma sağlanırsa bütün timerler durdurularak kaydetme aşamasına geçilir.

Uygulama başlatıldığında ise video kaynağından görüntüyü alır.Bu görüntü framelerden oluşuyor bunu Image<Bgr, byte> tipine dönüştürür.Böylece frameleri byte dizisi şekline getirilir.Daha sonra dönüştürülen bu görüntü işlenebilmek için grayscale'e dönüştürülür.

Görüntünün etrafında ise timer kullanılarak arkaplan gri veya beyaz değişimi yapılarak ilgili ortamda belli aydınlık sağlanarak bulunan ortama yardımcı olmaktadır. 

Tanıma bölümünde ise eşleşen kayıt var ise yüz tespiti için haarcascade_frontalface_default.xml dosyası kullanılır.haarcascade_frontalface_default.xml dosyasından Image byteları aranarak bulunan yüzler döndürülür.Ve başlangıçta yakaladığımız binary görüntü üzerine yazılır.Tanıma sağlandığında otomatik kullanıcı ismi gelerek tekrar program sonlandılır.

En son olarak windows user32.dll ve kernel32.dll kullanılarak kaydetme aşamasında klavye tuşlarını pasif edilir.Tam kontrol sağlanmış olur.

