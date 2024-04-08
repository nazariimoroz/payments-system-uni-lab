using payments_system_uni_lab.Users;

namespace payments_system_uni_lab.UI.Main
{
    /// <summary>
    /// Interaction logic for ClientMainUI.xaml
    /// </summary>
    public partial class ClientMainUI : UserMainUI
    {
        public override BaseUser User
        {
            get => _user;
            set
            {
                if(!(value is Client client))
                {
                    throw new BadUserException("User should be derived from client");
                }
                _user = value;

                ClientPhone.Text = client.PhoneNumber;
            }
        }
        
        public ClientMainUI()
        {
            InitializeComponent();
        }

        private BaseUser _user;
    }
}
