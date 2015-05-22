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
