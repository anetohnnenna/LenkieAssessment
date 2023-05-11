namespace LibraryApi.Entities
{

    public class ResponseCodes
    {
        public static string Success
        {
            get
            {
                return "00";
            }
        }

        public static string Failure
        {
            get
            {
                return "99";
            }
        }

        public static string TimeOut
        {
            get
            {
                return "202";
            }
        }

        public static string EmptyRecord
        {
            get
            {
                return "505";
            }
        }

    }
}
