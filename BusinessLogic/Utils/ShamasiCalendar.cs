using System.Globalization;
namespace BusinessLogic.Utils
{
    public class ShamasiCalendar
    {
        public string ToShamsi(DateTime dt)
        {
            var persianCalendar = new PersianCalendar();

            return string.Format(@"{0}/{1}/{2}",
                persianCalendar.GetYear(dt),
                persianCalendar.GetMonth(dt),
                persianCalendar.GetDayOfMonth(dt));

        }

    }
}
