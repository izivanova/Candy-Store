﻿using CandyStore.Client.Cache;
using CandyStore.Client.Forms;
using CandyStore.Client.Messages;
using CandyStore.DataModel;
using CandyStore.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CandyStore.Client
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void adminLoginButton_Click(object sender, EventArgs e)
        {
            var parsed = int.TryParse(identificationNumberBox.Text, out int identificationNumber);
            if (!parsed)
            {
                Logger.ShowError("Enter a correct whole number value.");
                return;
            }

            using (var context = new CandyStoreDbContext())
            {
                var userFromDb = context.Employees.FirstOrDefault(u => u.IdentificationNumber == identificationNumber);
                if (userFromDb == null)
                {
                    Logger.ShowError("There is no such employee");
                    return;
                }

                var adminPanelForm = new AdminPanelForm();
                adminPanelForm.Show();
                this.Hide();
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            customerGroupBox.BackColor = Color.Transparent;
            adminGroupBox.BackColor = Color.Transparent;

            firstnameLabel.BackColor = Color.Transparent;
            lastnameLabel.BackColor = Color.Transparent;
        }

        private void customerContinueBtn_Click(object sender, EventArgs e)
        {
            if (firstNameBox.Text == "" || lastNameBox.Text == "")
            {
                Logger.ShowError("Some of the values are empty");
                return;
            }
            Session.FirstName = firstNameBox.Text;
            Session.LastName = lastNameBox.Text;
            Session.Products = new Dictionary<Product, int>();

            var orderForm = new OrderCategoriesForm();
            orderForm.Show();
            this.Hide();
        }
    }
}