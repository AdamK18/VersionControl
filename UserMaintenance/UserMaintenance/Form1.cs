using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        BindingList<User> users = new BindingList<User>();

        public Form1()
        {
            InitializeComponent();

            lblFullName.Text = Resource1.FullName;
            btnAdd.Text = Resource1.Add;
            writeBtn.Text = Resource1.Write;
            deleteBtn.Text = Resource1.Delete;

            // listbox1
            listUsers.DataSource = users;
            listUsers.ValueMember = "ID";
            listUsers.DisplayMember = "FullName";

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = txtFullName.Text
            };
            users.Add(u);
        }

        private void writeBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            // set a default file name
            savefile.FileName = "asd.txt";
            // set filters - this can be done in properties as well
            savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(savefile.FileName);
                for (int i = 0; i < users.Count; i++)
                {
                    sw.WriteLine(users[i].ID + " " + users[i].FullName);
                }
                sw.Close();
            }
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if(users.Count > 0) users.RemoveAt(listUsers.SelectedIndex);
        }
    }
}
