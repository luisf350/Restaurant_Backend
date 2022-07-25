namespace Restaurant.Backend.Common.Constants
{
    public static class Constants
    {
        public static string NotFound => "Record was not found ({0}).";
        public static string ModelNotValid => "The Model is not valid.";
        public static string OperationNotCompleted => "The operation was not completed.";
        public static string LoginNotValid => "The Email and/or Password are not valid.";
        public static string EmailInUse => "The Email is in use already.";
        public static string CustomerNotActive => "The Customer is not active.";
        public static string SendNewKeyForActivation => "Please request new key for activation.";
        public static string ErrorFromNexmo => "CustomerId={0}. PhoneNumber={1}. Error Message= {2}.";
        public static string MissingNexmoKey => "The key for Nexmo is missing.";
    }
}
