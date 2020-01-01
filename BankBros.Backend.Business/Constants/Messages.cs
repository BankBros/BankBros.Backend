using System;
using System.Collections.Generic;
using System.Text;
using BankBros.Backend.Core.Utilities.Security.Jwt;
using BankBros.Backend.Entity.Concrete;

namespace BankBros.Backend.Business.Constants
{
    public class Messages
    {
        public static string UserNotFound = "Kullanıcı bulunamadı.";
        public static string PasswordError = "Şifre yanlış.";
        public static string LoginSuccessful = "Giriş işlemi başarılı.";
        public static string UserAlreadyExists = "Böyle bir kullanıcı mevcut.";
        public static string UserAvailable = "Kullanıcı mevcut.";
        public static string RegisterSucessfull = "Kayıt işlemi başarıyla tamamlandı.";
        public static string RegisterFailed = "Kayıt işlemi başarısız.";
        public static string AccessTokenCreated = "Access Token oluşturuldu.";
        public static string AccessTokenFailed = "Acces Token oluşturulamadı.";
        public static string UserBanned = "Bu kullanıcı askıya alınmıştır.";
        public static string AccountAddedSuccessfully = "Hesap ekleme işlemi başarıyla tamamlandı.";
        public static string AccountAddingFailure = "Hesap ekleme işlemi başarısız oldu.";
        public static string AccountUpdated = "Hesap güncellendi.";
        public static string AccountUpdateFailure = "Hesap güncelleme işlemi başarısız.";
        public static string AccountBalanceIsNotEqualsZero = "Hesap bakiyesi 0(Sıfır) değil.";
        public static string AccountDeleted = "Hesap silindi.";
        public static string AccountDeletedFailure = "Hesap silme işlemi başarısız.";
        public static string Listed = "Kullanıcıya ait {0} hesap listelendi";
        public static string AccountNotFound = "Hesap bulunamadı.";
        public static string InvalidToken = "Geçersiz token.";
        public static string UnsufficientBalance = "Yetersiz bakiye.";
        public static string TransactionNotFound = "İşlem bulunamadı.";
        public static string TransferSuccessful = "Havale işlemi başarıyla tamamlandı.";
        public static string DrawSuccessful = "Para çekme işlemi başarılı.";
        public static string DepositSuccessful = "Para yatırma işlemi başarılı.";
        public static string AccountIsPassive = "Hesap pasif durumdadır. İşlem başarısız.";
        public static string BalanceTypesNotReachable = "Hesap tiplerine erişilemedi.";
        public static string TransactionTypesNotReachable = "İşlem tiplerine erişlemedi.";
        public static string TransactionResultsNotReachable = "İşlem sonuçlarına erişilemedi.";
        public static string TransferFailed = "Havale işlemi başarısız.";
        public static string VirementFailed = "Virman işlemi başarısız.";
        public static string VirementSuccessful = "Virman işlemi başarıyla tamamlandı.";
        public static string AuthorizationDenied = "Yetkisiz eylem.";
        public static string ApplicationNotFound = "Uygulama bulunamadı.";
        public static string UserLogsNotFound = "Kullanıcı kayıtları bulunamadı.";
        public static string CreditRequestAddedSuccessfully = "Kredi isteği başarıyla eklendi.";
        public static string CreditRequestAddingFailure = "Kredi isteği ekleme işlemi başarısız.";
        public static string CreditRequestUpdateFailure = "Kredi isteği güncelleme işlemi başarısız.";
        public static string CreditRequestUpdated = "Kredi isteği güncelleme başarılı.";
        public static string CreditRequestDeleted = "Kredi isteği silme işlemi başarılı.";
        public static string CreditRequestDeleteFailure = "Kredi isteği silme işlemi başarısız.";
        public static string CreditRequestAlreadyExists = "Kredi isteği zaten mevcut.";
        public static string CreditRequestNotExists = "Kredi isteği mevcut değil.";
    }
}
