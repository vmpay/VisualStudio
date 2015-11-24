using Template.Components.Mail;
using Xunit;

namespace Template.Tests.Unit.Components.Mail
{
    public class SmtpMailClientTests
    {
        #region Dispose()

        [Fact]
        public void Dispose_MultipleTimes()
        {
            SmtpMailClient client = new SmtpMailClient();

            client.Dispose();
            client.Dispose();
        }

        #endregion
    }
}
