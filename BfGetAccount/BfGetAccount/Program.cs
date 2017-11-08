namespace BfGetAccount
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var bfContext = new BfContext();
            bfContext.Passwords.Add(new Password()
            {
                Text = "123456789"
            });
            bfContext.SaveChanges();
        }
    }
}