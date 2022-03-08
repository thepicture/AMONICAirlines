using AMONICAirlinesDesktopApp.Models.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AMONICAirlinesDesktopApp.ViewModels
{
    public class AdministratorMainMenuViewModel : BaseViewModel
    {
        public AdministratorMainMenuViewModel()
        {
            Title = "AMONIC Airlines Automation System";
            Users = Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context.User
                    .Include(u => u.Role)
                    .Include(u => u.Office)
                    .ToList();
                }
            }).Result;
            var offices = Task.Run(() =>
            {
                using (BaseEntities context = new BaseEntities())
                {
                    return context.Office
                    .ToList();
                }
            }).Result;
            offices.Insert(0,
                           new Office
                           {
                               Title = "All offices"
                           });
            Offices = offices;
        }

        private IEnumerable<User> users;

        public IEnumerable<User> Users
        {
            get => users;
            set => SetProperty(ref users, value);
        }

        private IEnumerable<Office> offices;

        public IEnumerable<Office> Offices
        {
            get => offices;
            set => SetProperty(ref offices, value);
        }
    }
}
