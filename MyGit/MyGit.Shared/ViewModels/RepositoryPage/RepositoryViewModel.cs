using System.Threading.Tasks;
using Octokit;

namespace MyGit.ViewModels.RepositoryPage
{
    public class RepositoryViewModel : BaseViewModel
    {
        private Repository _repository;

        public Repository Repository
        {
            get { return _repository; }
            set
            {
                _repository = value; 
                OnPropertyChanged();
            }
        }

        private readonly string _owner;
        private readonly string _name;

        public RepositoryViewModel(string owner, string name) : this()
        {
            _owner = owner;
            _name = name;
        }

        public RepositoryViewModel()
        {
            
        }

        public override async Task Refresh()
        {
            Repository = await GitHubClient.Repository.Get(_owner, _name);
        }
    }
}
