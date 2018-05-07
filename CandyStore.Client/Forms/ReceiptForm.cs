﻿using CandyStore.Client.Cache;
using CandyStore.Client.Util;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CandyStore.Client.Forms
{
    public partial class ReceiptForm : Form
    {
        private double _totalPrice;
        private int _orderID;
        public ReceiptForm(int orderID, double totalPrice)
        {
            _totalPrice = totalPrice;
            _orderID = orderID;
            InitializeComponent();
            receiptTextBox.SelectionAlignment = HorizontalAlignment.Center;

            CandyStoreUtil.MakeLabelsTransparent(this);
        }

        private void nextCustomerButton_Click(object sender, EventArgs e)
        {
            Session.Clear();
            this.Hide();
            var startupForm = Application.OpenForms.OfType<Main>().FirstOrDefault();

            startupForm = startupForm == null ? new Main() : startupForm;
            startupForm.Show();
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            receiptTextBox.Text += "Candy Store Receipt\n";
            receiptTextBox.Text += "\n";
            receiptTextBox.Text += "Order: " + _orderID.ToString() + "\n";

            foreach (var item in Session.Products)
            {
                string currentLine = $"{item.Key.Name}  ${item.Key.Price:f2} x {item.Value}    ${item.Value * item.Key.Price:f2}" + "\n";
                receiptTextBox.Text += currentLine;
            }

            receiptTextBox.Text += "\nTotal price: " + _totalPrice.ToString("0.00" + "$") + "\n";
            receiptTextBox.ReadOnly = true;

            receiptTextBox.Text += $"\nCustomer: {Session.FirstName} {Session.LastName}\n";
            receiptTextBox.Text += $"\nDate: {DateTime.Now}";
        }
    }
}