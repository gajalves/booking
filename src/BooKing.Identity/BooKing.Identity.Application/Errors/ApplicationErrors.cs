using BooKing.Generics.Domain;

namespace BooKing.Identity.Application.Erros;
public static class ApplicationErrors
{
    public static class UserError
    {
        public static readonly Error EmailAlreadyInUse = new Error(
            "UserRegisterService.Register",
            "The provided email address is already in use!");

        public static readonly Error UnexpectedErrorCreatingUser = new Error(
            "UserRegisterService.Register",
            "Unexpected error while creating user!");

        public static readonly Error ProvidedEmailAccountNotFound = new Error(
            "UserLoginService.Login",
            "Provided email account does not exists!");

        public static readonly Error PasswordIncorrect = new Error(
            "UserLoginService.Login",
            "Password Incorrect!");

        public static readonly Error NotAllowedToRetrieveThisInformation = new Error(
            "UserService",
            "You're not allowed to retrieve this information!");
    }
}
