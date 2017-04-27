using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;

namespace SportNotes.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class SportUser : IdentityUser
    {
        public SportUser()
        {
            this.Notes = new HashSet<Note>();
        }

        public virtual ICollection<Note> Notes { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SportUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class SportNotesDbContext : IdentityDbContext<SportUser>
    {
        public SportNotesDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Note> Notes { get; set; }

        public static SportNotesDbContext Create()
        {
            return new SportNotesDbContext();
        }
    }
}