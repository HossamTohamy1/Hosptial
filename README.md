# 🏥 Hospital Management System

> A website that helps a hospital keep patient records, doctors, diagnoses and prescriptions in one place.

---

## 🧐 What is this, in plain words?

In many clinics, patient files are still paper folders — easy to lose, hard to search, and impossible to share between doctors quickly.

This project is a **complete web application** (pages + logic) that replaces those folders:

- 🧑‍🤝‍🧑 **Patients** — register patients and keep their records.
- 👨‍⚕️ **Doctors** — manage the hospital's doctors.
- 🩺 **Visits & diagnoses** — record each visit and what the doctor found.
- 💊 **Medicines & prescriptions** — keep a list of medicines and write prescriptions for patients.
- 🔐 **Accounts & roles** — staff log in, and each role only sees what it should.

## 👥 Who uses it?

| Who | What they can do |
|---|---|
| **Admin** | Manage doctors, medicines, users and roles |
| **Doctor** | View patients, add diagnoses, write prescriptions |
| **Reception** | Register patients and their visits |

## 🔄 How it works (the simple story)

```
Patient arrives ➜ Reception registers the visit ➜ Doctor examines and records a diagnosis
        ➜ Doctor prescribes medicine ➜ Everything is saved in the patient's record
```

---

## 🛠️ For developers

**Tech stack:** ASP.NET Core MVC (.NET 9) · Razor views · ASP.NET Core Identity (login & roles) · Entity Framework Core + SQL Server · SMTP email (forgot/reset password)

**Main entities:** `Patient`, `Doctor`, `Visitation`, `Diagnosis`, `Medicament`, `Prescriptions`, `PatientMedicament`, `ApplicationUser`

**Run locally**

1. Install the [.NET 9 SDK](https://dotnet.microsoft.com/download) and SQL Server.
2. In `appsettings.json`, replace `CHANGE_ME` with your DB password and SMTP sender password.
3. Run:
   ```bash
   dotnet ef database update
   dotnet run
   ```
4. Open the URL shown in the terminal.

---

## 🇪🇬 بالعربي

موقع لإدارة مستشفى بدل الملفات الورق: بيسجّل المرضى والدكاترة، وكل زيارة والتشخيص بتاعها، والأدوية والروشتات. وكل موظف بيدخل بحسابه ويشوف اللي يخصه بس.
