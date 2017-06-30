using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;

namespace HackAtHomeClient
{
    [Activity(Label = "Hack@Home", MainLauncher = true, Icon = "@drawable/icon_bol")]
    public class MainActivity : Activity
    {
        TextView txtEmail;
        TextView txtPwd;
        Button btnValidate;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            btnValidate = FindViewById<Button>(Resource.Id.buttonValidate);
            txtEmail = FindViewById<TextView>(Resource.Id.txtEmail);
            txtPwd = FindViewById<TextView>(Resource.Id.txtPassword);
            btnValidate.Click += BtnValidate_Click;
        }

        private async void BtnValidate_Click(object sender, System.EventArgs e)
        {
            var client = new ServiceCaller();
            if (string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPwd.Text))
            {
                showAlert(Resources.GetString(Resource.String.error), Resources.GetString(Resource.String.errorInfo));
                return;
            }
            var result = await client.AutentificateAsync(txtEmail.Text, txtPwd.Text);
            if (result.Status == HackAtHome.Entities.Status.Success)
            {
                var evidence = new Android.Content.Intent(this, typeof(EvidenceActivity));
                evidence.PutExtra("userName", result.FullName);
                evidence.PutExtra("token", result.Token);
                StartActivity(evidence);
            }
            else
            {
                showAlert(Resources.GetString(Resource.String.error), Resources.GetString(Resource.String.errorLogin));
            }
        }
        void showAlert(string title, string message)
        {
            var builder = new AlertDialog.Builder(this);
            var alert = builder.Create();
            alert.SetTitle(title);
            alert.SetIcon(Resource.Drawable.icon_bol);
            alert.SetMessage(message);
            alert.SetButton(Resources.GetString(Resource.String.aceptar), (s, ev) => { });
            alert.Show();
        }
    }
}

