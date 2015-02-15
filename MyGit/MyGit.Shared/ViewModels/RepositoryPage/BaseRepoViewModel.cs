using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyGit.ViewModels.RepositoryPage
{
    public abstract class BaseRepoViewModel : BaseViewModel
    {
        protected string Owner { get; set; }
        protected string Repo { get; set; }

        protected BaseRepoViewModel(string owner, string repo) : base()
        {
            Owner = owner;
            Repo = repo;
        }
    }
}
