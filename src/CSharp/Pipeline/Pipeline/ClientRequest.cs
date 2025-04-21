namespace Pipeline
{
    public class ClientRequest
    {
        #region Step1
        public string Email { get; set; }

        public string Passowrd { get; set; }
        #endregion

        #region Step2
        public string FirstName { get; set; }

        public string LastName { get; set; }
        #endregion

        #region Step3
        public string Phone { get; set; }
        #endregion
    }
}
