using System.Linq;

namespace AMONICAirlinesDesktopApp_Session3.Models.Entities
{
    public partial class Tickets
    {
        public Countries Country
        {
            get
            {
                using (SessionThreeEntities context =
                    new SessionThreeEntities())
                {
                    return context
                        .Countries
                        .First(c => c.ID == PassportCountryID);
                }
            }
        }
    }
}
