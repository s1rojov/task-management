# Task Management — frontend uchun texnik hujjat

Ushbu hujjat repositorydagi backend kodining 2026-10-21 holatiga asoslangan. Status controllerga qo‘shilgan `[ApiController]` va `[Route("api/[controller]")]` hisobga olindi: status endpointlari endi `/api/Status` prefiksiga ega. Tahlil statik kod tekshiruvi orqali bajarildi; API va ma’lumotlar bazasi ishga tushirib tekshirilmadi.

## 1. Loyiha vazifasi va mavjud imkoniyatlar

Loyiha vazifalarni foydalanuvchiga biriktirish va statuslar orqali boshqarish uchun ASP.NET Core Web API backendidir. Frontend kodi hozircha mavjud emas.

Backend texnologiyalari: .NET 8, Entity Framework Core 8, PostgreSQL uchun Npgsql va Swagger. Tuzilishi: `Controller → Service → Repository → ApplicationDbContext → PostgreSQL`.

| Obyekt | Mavjud amallar | Bog‘lanish |
| --- | --- | --- |
| Task — vazifa | Yaratish, ro‘yxat, ID bo‘yicha olish, yangilash, o‘chirish | Bitta user va bitta status |
| User — foydalanuvchi | Yaratish, ro‘yxat, ID bo‘yicha olish, yangilash, o‘chirish | Vazifaga mas’ul shaxs |
| Status — holat | Yaratish, ro‘yxat, ID bo‘yicha olish, yangilash, o‘chirish | Vazifaning holati |

Login, ro‘yxatdan o‘tish, JWT, rollar, loyiha/workspace, deadline, priority, izohlar, fayl biriktirish va bildirishnoma endpointlari mavjud emas. `UseAuthorization()` borligi login tizimi borligini anglatmaydi. `WeatherForecast` — shablondan qolgan endpoint; mahsulot interfeysiga kerak emas.

## 2. API ulanishi

`Task-management/Properties/launchSettings.json` dagi manzillar:

| Profil | Server manzili |
| --- | --- |
| https | `https://localhost:7018` va `http://localhost:5263` |
| http | `http://localhost:5263` |
| IIS Express | `https://localhost:44313` yoki `http://localhost:42027` |

HTTPS profilni ishga tushirish uchun repository ildizida:

```powershell
dotnet run --project Task-management/Task-management.csproj --launch-profile https
```

Bu buyruq uchun .NET 8 SDK, PostgreSQL ulanishi va mos jadvallar kerak. Repositoryda migrations va seed ma’lumotlari topilmadi; serverning o‘zi DB sxemasini avtomatik yaratmaydi. Ulanish konfiguratsiyasi backendda saqlanadi, frontendga DB rekvizitlari ko‘chirilmaydi.

Development muhitida Swagger: `https://localhost:7018/swagger`.

Frontend konfiguratsiyasidagi `API_BASE_URL` server originini saqlasin, masalan `https://localhost:7018`; endpoint yo‘llari quyidagi jadvallardagidek `/api/...` bilan boshlanadi. Yozish so‘rovlari uchun `Content-Type: application/json` yuboriladi. JSON maydonlari standart ASP.NET Core sozlamalariga ko‘ra `camelCase`; DB ustunlaridagi `snake_case` API formati emas.

**Integratsiya sharti:** `Program.cs` da CORS sozlanmagan. Frontend boshqa port/originda ishlasa, backendda uning originiga ruxsat berish yoki frontend dev serverida API proxy sozlash kerak. Serverda HTTPS redirect bor; ulanishni tanlangan profilning HTTPS manziliga moslash kerak.

## 3. Ma’lumotlar turlari

Quyidagi TypeScript turlari frontend uchun shartnoma namunasi; ular loyihaga qo‘shilgan kod emas.

```ts
export interface TaskItem {
  id: number;
  title: string;
  description: string | null;
  updatedAt: string; // ISO 8601 sana-vaqt
  userId: number;
  userName: string; // firstName + " " + lastName
  statusId: number;
  statusName: string;
}

export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  createdAt: string;
}

export interface TaskStatus {
  id: number;
  title: string;
}

export interface CreateTaskInput {
  title: string;
  description: string | null;
  userId: number;
  statusId: number;
}

export interface UpdateTaskInput extends CreateTaskInput {
  id: number;
}

export interface CreateUserInput {
  firstName: string;
  lastName: string;
  email: string;
}

export interface UpdateUserInput extends CreateUserInput {
  id: number;
  createdAt: string; // mavjud qiymatni saqlab yuborish kerak
}
```

User va status matn maydonlarida backend validatsiyasi kuchsiz: eski/noto‘g‘ri yozuvlar null bo‘lishi mumkin. Interfeys bunday qiymatlar uchun `—` kabi fallback ko‘rsatsin. Yuqoridagi turlar to‘g‘ri to‘ldirilgan yozuvlar uchun mo‘ljallangan.

## 4. Endpointlar va javoblar

### Vazifalar

| Method | Yo‘l | Body | Muvaffaqiyatli javob |
| --- | --- | --- | --- |
| GET | `/api/Task/GetList` | — | `200`, `TaskItem[]` |
| GET | `/api/Task/Get/{id}` | — | `200`, `TaskItem` |
| POST | `/api/Task/Create` | `CreateTaskInput` | `201`, `Task addded` matni |
| PUT | `/api/Task/Update` | `UpdateTaskInput` | `200`, yuborilgan DTO qaytariladi |
| DELETE | `/api/Task/Delete/{id}` | — | `200`, o‘chirilgan `TaskModel` |

Yaratish namunasi:

```json
{
  "title": "Vazifalar sahifasini tayyorlash",
  "description": "Ro‘yxat va yaratish formasini ulash",
  "statusId": 1,
  "userId": 2
}
```

`statusId` va `userId` namunaviy; haqiqiy ro‘yxatlardan tanlangan mavjud ID yuboriladi.

Yangilash namunasi:

```json
{
  "id": 5,
  "title": "Vazifalar sahifasini tayyorlash",
  "description": "Ro‘yxat tayyor, forma jarayonda",
  "statusId": 2,
  "userId": 2
}
```

Task olish javobining namunasi:

```json
{
  "id": 5,
  "title": "Vazifalar sahifasini tayyorlash",
  "description": "Ro‘yxat tayyor, forma jarayonda",
  "updatedAt": "2026-10-21T09:00:00Z",
  "userId": 2,
  "userName": "Ali Valiyev",
  "statusId": 2,
  "statusName": "Jarayonda"
}
```

Task integratsiyasining muhim xususiyatlari:

- Create action amalda `TaskListDto` qabul qiladi, lekin faqat `title`, `description`, `statusId`, `userId` ni modelga ko‘chiradi. `id`, `updatedAt`, `userName`, `statusName` yaratishda yuborilishi shart emas.
- Create javobi obyekt yoki yangi ID emas. `Task addded` yozilishi backenddagi aynan mavjud matn. Javobni doim JSON obyekt deb o‘qimaslik kerak; `Content-Type` asosida matn/JSON farqlansin.
- Update bazadagi `updatedAt` ni `DateTime.UtcNow` bilan o‘zgartiradi, ammo javobga qayta o‘qilgan yozuv o‘rniga kiruvchi DTOni qaytaradi. Javobdagi sana va nomlarga tayanilmasin. Saqlagandan keyin `GetList` yoki `Get/{id}` qayta chaqirilsin.
- Update to‘liq tahrirlash sifatida ishlatiladi. Faqat status o‘zgarganda ham mavjud `title`, `description`, `userId` va yangi `statusId` bilan `id` yuborilsin; aks holda qolgan maydonlar default qiymatga tushishi mumkin.
- Delete javobi `TaskItem` bilan bir xil emas: `TaskModel` ichida `status` va `user` navigation maydonlari bor, ular yuklanmagani sababli null bo‘lishi mumkin. UI uchun muvaffaqiyat kodi yetarli; so‘ng ro‘yxat qayta yuklansin.

### Foydalanuvchilar

| Method | Yo‘l | Body | Muvaffaqiyatli javob |
| --- | --- | --- | --- |
| GET | `/api/User/GetList` | — | `200`, `User[]` |
| GET | `/api/User/Get/{userId}` | — | `200`, `User` mavjud bo‘lsa |
| POST | `/api/User/Create` | `CreateUserInput` | `201`, yaratilgan `User` |
| PUT | `/api/User/Update` | `UpdateUserInput` | `200`, `User` |
| DELETE | `/api/User/Delete/{id}` | — | `200`, o‘chirilgan `User` |

Yaratish namunasi:

```json
{
  "firstName": "Ali",
  "lastName": "Valiyev",
  "email": "ali@example.com"
}
```

Yaratishda `id` va `createdAt` ni yubormaslik mumkin. Yangilashda esa olingan `createdAt` qiymati saqlab yuborilsin: repository butun modelni yangilaydi; maydon yuborilmasa model initializer hozirgi vaqtni qo‘yib, asl yaratilish sanasini o‘zgartirishi mumkin.

### Statuslar

| Method | Yo‘l | Body | Muvaffaqiyatli javob |
| --- | --- | --- | --- |
| GET | `/api/Status/GetList` | — | `200`, `TaskStatus[]` |
| GET | `/api/Status/Get/{statusId}` | — | `200`, `TaskStatus` mavjud bo‘lsa |
| POST | `/api/Status/Create` | `{ "title": "Jarayonda" }` | `201`, yaratilgan `TaskStatus` |
| PUT | `/api/Status/Update` | `{ "id": 2, "title": "Yakunlandi" }` | `200`, `TaskStatus` |
| DELETE | `/api/Status/Delete/{statusId}` | — | `200`, o‘chirilgan `TaskStatus` |

Statuslar bazadan keladigan dinamik ma’lumot. `1 = Yangi`, `2 = Jarayonda`, `3 = Yakunlandi` kabi doimiy ID mapping ishlatilmasin. Bu nomlar faqat UI misollari; seed yoki yakunlanganlikni belgilovchi alohida maydon yo‘q.

## 5. Frontend sahifalari va oqimlari

Quyidagi sahifalar — mavjud API asosida frontend uchun taklif etilayotgan talablar. Ular backendda allaqachon mavjud UI emas.

| Frontend yo‘li | Mazmuni |
| --- | --- |
| `/tasks` | Vazifalar ro‘yxati, qidiruv, status/user filtri, yaratish va tahrirlash |
| `/tasks/:id` | Vazifa tafsilotlari va tahrirlash; alohida sahifa o‘rniga modal ham mumkin |
| `/users` | Foydalanuvchilar ro‘yxati va CRUD formasi |
| `/statuses` | Statuslar ro‘yxati va CRUD formasi |
| `/` | `/tasks` ga yo‘naltirish |

### Vazifalar ro‘yxati

Jadval ustunlari: sarlavha, mas’ul shaxs, status, oxirgi yangilangan vaqt va amallar. Tavsif tafsilotlarda to‘liq ko‘rsatiladi. Kichik ekranda kartochka ko‘rinishi yoki gorizontal scroll ishlatilishi mumkin.

Boshlang‘ich yuklashda task, user va status ro‘yxatlari olinadi. Mustaqil so‘rovlarni parallel bajarish mumkin. Backendda qidiruv, filter, saralash va pagination parametrlari yo‘q: boshlang‘ich versiyada ular yuklangan ro‘yxat ustida frontendda bajariladi. APIga `page`, `search` yoki `statusId` qo‘shish natijani filtrlamaydi. Ro‘yxat tartibi kafolatlanmagan; frontend kerak bo‘lsa `updatedAt` bo‘yicha saralasin.

### Vazifa yaratish va tahrirlash

Forma maydonlari: sarlavha, ixtiyoriy tavsif, user select va status select. Select qiymatlari songa aylantirib yuboriladi. Status yoki user ro‘yxati bo‘sh bo‘lsa, saqlash bloklanib, avval tegishli yozuvni yaratish uchun havola ko‘rsatiladi.

Yaratish/tahrirlash oqimi:

1. Formani tekshirish va yuborish tugmasini vaqtincha bloklash.
2. Mos POST yoki PUT so‘rovini yuborish.
3. Muvaffaqiyatda xabar chiqarish va task ro‘yxatini qayta olish.
4. Xatoda foydalanuvchi kiritgan qiymatlarni saqlash va qayta urinishga ruxsat berish.

O‘chirish oldidan obyekt nomi bilan tasdiqlash dialogi ko‘rsatilsin. Muvaffaqiyatli javob kelmaguncha yozuv interfeysdan butunlay olib tashlanmasin.

### Foydalanuvchi va statuslarni boshqarish

User formasida ism, familiya va email; status formasida nom bo‘ladi. User/status nomi yangilanganda task ro‘yxati ham qayta olinsin: undagi `userName` va `statusName` yangilanishi kerak.

Bog‘langan user/statusni o‘chirish oqibati uchun maxsus biznes qoida yozilmagan. EF ning required relationship konvensiyasi kaskad o‘chirishga olib kelishi mumkin; haqiqiy DB sxemasi tekshirilmagan. Frontendda foydalanilayotgan user/status uchun o‘chirishni bloklash — vaqtinchalik UX chorasi. Backend darajasida bog‘langan vazifalarni himoyalash qoidasi alohida belgilanishi kerak; faqat frontend tekshiruvi yetarli emas.

## 6. Validatsiya va interfeys holatlari

Frontend uchun forma qoidalari:

- Task sarlavhasi trimdan keyin bo‘sh bo‘lmasin; 200 belgilik limit qo‘llansin.
- Task user va statusi mavjud ro‘yxatdan tanlansin; ID musbat butun son bo‘lsin.
- Tavsif ixtiyoriy, bo‘sh bo‘lsa `null` yuborish mumkin.
- Ism, familiya va status nomi trimdan keyin bo‘sh bo‘lmasin; email formati tekshirilsin.
- Takroriy email yoki status nomi bo‘yicha backendda aniq unique validatsiya ko‘rinmaydi; server buni kafolatlaydi deb qabul qilinmasin.

200 belgilik limit `CreateTaskDto` dagi niyatga mos frontend qoidasi. **Bu DTO controllerda ishlatilmayapti**, shuning uchun uning `[Required]` va `[MaxLength(200)]` atributlari amaldagi task create endpointida bajarilmaydi. Frontend validatsiyasi server validatsiyasini almashtirmaydi.

Har sahifada loading, bo‘sh ro‘yxat, filtr natijasi bo‘shligi va yuklash xatosi alohida ko‘rsatilsin. Xatoda qayta yuklash tugmasi bo‘lsin. Forma label va xatolari maydonlar bilan bog‘lansin, modal klaviatura orqali ishlasin. Sanalar serverdan kelgan ISO qiymat sifatida saqlanib, foydalanuvchining lokal vaqtida formatlansin.

## 7. Xatolarni qayta ishlash

Yagona error-response shartnomasi va global exception handler mavjud emas.

| Holat | Koddan kuzatiladigan xulq | Frontend yondashuvi |
| --- | --- | --- |
| Noto‘g‘ri JSON yoki tip | `[ApiController]` model binding orqali 400 qaytarishi mumkin | Mavjud bo‘lsa `errors` maydonini o‘qish |
| Task ID topilmadi | `Exception("Task not found")`; 404 ga mapping yo‘q, odatda 500 | Faqat 404 ni kutib qolmaslik |
| User/status ID topilmadi | `Ok(null)`; standart formatter 204 bo‘sh javob qaytarishi mumkin | Bo‘sh body/nullni topilmadi sifatida ko‘rsatish |
| Mavjud bo‘lmagan yozuvni o‘chirish | Null modelni o‘chirishga urinish exception berishi mumkin | Xatoni ko‘rsatish va ro‘yxatni yangilash |
| Noto‘g‘ri bog‘langan ID yoki DB xatosi | Maxsus mapping yo‘q, 500 bo‘lishi mumkin | Umumiy tushunarli xabar |
| CORS, sertifikat yoki tarmoq muammosi | Brauzer javobni o‘qiy olmasligi mumkin | Ulanish xabari va qayta urinish |

Umumiy API qatlami avval status va body mavjudligini tekshirsin; JSON, matn va bo‘sh javobni ajratsin. Har javobda ko‘r-ko‘rona `response.json()` chaqirish create task va 204 holatida xato beradi. Server stack trace yoki HTML xato sahifasi foydalanuvchiga chiqarilmasin.

## 8. Frontend kodini tashkil qilish

Framework tanlovi bu hujjat doirasida belgilanmagan. Quyidagi ajratish istalgan frontendda qo‘llanishi mumkin:

```text
src/
  api/          # umumiy HTTP client; tasks, users, statuses so‘rovlari
  types/        # API turlari va forma inputlari
  pages/        # Tasks, TaskDetails, Users, Statuses
  components/   # formalar, jadval, modal, select, xabarlar
  utils/        # sana formatlash, filter va validatsiya
```

Base URL bitta konfiguratsiyada bo‘lsin. Har component ichida endpoint satrlarini takrorlash o‘rniga API funksiyalaridan foydalanilsin. Backend javoblarining farqlari API qatlamida boshqarilsin. Birinchi versiyada server javobidan keyin qayta yuklash oqimi yetarli; murakkab optimistic update talab qilinmaydi.

## 9. Integratsiyadan oldingi backend masalalari

Status prefiksi tuzatilgan; frontend `/api/Status/...` yo‘llaridan foydalanadi. Qolgan masalalar:

| Masala | Frontendga ta’siri | Backendda kerakli yaxshilash |
| --- | --- | --- |
| CORS yo‘q | Alohida origin so‘rovlari bloklanishi mumkin | Frontend originini ruxsat etish yoki proxy |
| Task create matn qaytaradi | Yangi ID darhol mavjud emas | Yaratilgan DTO va ID qaytarish |
| Task update kiruvchi DTOni qaytaradi | Sana/nomlar eskirgan yoki default | Yangilangan DTOni qayta o‘qib qaytarish |
| CreateTaskDto ishlatilmaydi | E’lon qilingan validatsiya bajarilmaydi | Create/update uchun alohida DTO va validatsiya |
| 404 va error format yo‘q | Xatoni aniq ajratish qiyin | NotFound va yagona xato formati |
| User update butun modelni oladi | `createdAt` tasodifan o‘zgarishi mumkin | Faqat tahrirlanadigan maydonlarni yangilash |
| Delete bog‘lanish siyosati aniq emas | Bog‘liq vazifalarni yo‘qotish ehtimoli | Restrict/cascade biznes qoidasini belgilash |
| Auth mavjud emas | Haqiqiy kirish cheklovi yo‘q | Faqat mahsulot talabi bo‘lsa auth qo‘shish |

Bu hujjat backendni o‘zgartirmaydi; jadval joriy integratsiya cheklovlarini ko‘rsatadi.

## 10. Frontendni qabul qilish mezonlari

- [ ] Task, user va status ro‘yxatlari haqiqiy API bilan ishlaydi.
- [ ] Status so‘rovlari `/api/Status` prefiksidan foydalanadi.
- [ ] Barcha uch obyekt uchun yaratish, tahrirlash va o‘chirish oqimlari mavjud.
- [ ] Task create matn javobi va bo‘sh javoblar parsing xatosiga olib kelmaydi.
- [ ] Task tahriridan keyin serverdan yangi sana, user nomi va status nomi olinadi.
- [ ] User tahririda dastlabki `createdAt` saqlanadi.
- [ ] Status ID va nomlari hardcode qilinmagan.
- [ ] Qidiruv/filter/saralash clientda bajarilishi hisobga olingan.
- [ ] Loading, empty, error, saqlash jarayoni va qayta urinish holatlari ishlaydi.
- [ ] User/status bo‘lmasa task yaratish bo‘yicha aniq ko‘rsatma chiqadi.
- [ ] Bog‘langan user/status o‘chirish siyosati integratsiyada tekshiriladi.
- [ ] Mobil ekran, klaviatura boshqaruvi va sana formati tekshiriladi.

## 11. Tahlil manbalari

- `Task-management/Controllers/{Task,User,Status}Controller.cs` — route, request va response.
- `Task-management/DTO/Tasks/{CreateDto,GetListDto}.cs` — DTO va validatsiya deklaratsiyalari.
- `Task-management/Models/{Task,User,Status}Model.cs` — maydonlar va bog‘lanishlar.
- `Task-management/Services/Tasks/TaskService.cs` — task javobiga user/status nomlarini qo‘shish.
- `Task-management/Repositories/` — saqlash, yangilash, o‘chirish va topilmagan ID xulqi.
- `Task-management/Data/ApplicationDbContext.cs` — EF konteksti.
- `Task-management/Program.cs` — middleware, Swagger, CORS/auth holati va DB provider.
- `Task-management/Properties/launchSettings.json` — lokal manzillar.
- `Task-management/Task-management.csproj` — .NET va paketlar.
