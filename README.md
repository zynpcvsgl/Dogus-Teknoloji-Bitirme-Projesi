# 📚 BlogApp - Doğuş Teknoloji Bootcamp Bitirme Projesi

Bu proje, Doğuş Teknoloji Bootcamp kapsamında geliştirilmiş bir **Blog Uygulaması**dır. ASP.NET Core MVC altyapısıyla inşa edilen bu uygulama, kullanıcıların blog gönderileri oluşturmasını, kategorilere göre filtrelemesini ve diğer kullanıcılarla etkileşime geçmesini sağlar.

---

## 🚀 Özellikler

### 👤 Kullanıcı Yönetimi
- Üye olma, giriş ve çıkış işlemleri  
- Yetkilendirme: Sadece giriş yapan kullanıcılar yazı oluşturabilir, düzenleyebilir ve silebilir  
- Rol bazlı erişim: Admin paneli ile içerik ve yorum kontrolü  

### ✍️ Blog İşlemleri
- Blog gönderisi ekleme, düzenleme, silme (CRUD)  
- Gönderiye görsel ekleme (IFormFile)  
- Yayınlanma tarihi, kategori ve kullanıcı bilgileriyle ilişkilendirme  
- Detay sayfası üzerinden yazıların okunması  

### 🗂 Kategori Sistemi
- Bloglar kategoriye göre filtrelenebilir  
- Kategoriler dinamik olarak veri tabanından çekilir  

### 💬 Yorum Sistemi
- Sadece giriş yapmış kullanıcılar yorum bırakabilir  
- Admin paneli üzerinden yorumları onaylama veya silme  

### 🎯 Arama & Etiketleme
- Anahtar kelimeye göre yazı arama  
- Etiketler ile blogları filtreleme  

---

## 🛠️ Kullanılan Teknolojiler
- **ASP.NET Core MVC**  
- **Entity Framework Core**  
- **Razor Pages & Bootstrap**  
- **MSSQL** (SQLite opsiyonel)  
- **Cookie Authentication**  
- **Dependency Injection & Repository Pattern**

---

## ⚙️ Kurulum

1. Bu repoyu klonlayın:
   ```
   git clone https://github.com/zynpcvsgl/BlogApp.git
   ```

2. Gerekli NuGet paketlerini yükleyin.

3. `appsettings.json` dosyasındaki veritabanı bağlantı cümlesini kendi ortamınıza göre güncelleyin.

4. Migration ve veritabanı oluşturmak için:
   ```
   dotnet ef database update
   ```

5. Uygulamayı başlatmak için:
   ```
   dotnet run
   ```

---
