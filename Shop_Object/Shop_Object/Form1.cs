using System;
using System.Windows.Forms;

namespace Shop_Object
{
    public partial class Form1 : Form
    {
        Item itemCoffee = new Item();
        Item itemGreenTea = new Item();
        Item itemNoodle = new Item();
        Item itemPizza = new Item();

        public Form1()
        {
            InitializeComponent();

            // Add data to objects
            itemCoffee.name = "Coffee";
            itemCoffee.price = 75;
            itemCoffee.quantity = 0;

            itemGreenTea.name = "GreenTea";
            itemGreenTea.price = 60;
            itemGreenTea.quantity = 0;

            itemNoodle.name = "Noodle";
            itemNoodle.price = 120;
            itemNoodle.quantity = 0;

            itemPizza.name = "Pizza";
            itemPizza.price = 250;
            itemPizza.quantity = 0;

            // Display data
            CoffeePrice.Text = itemCoffee.price.ToString();
            CoffeeQuantity.Text = itemCoffee.quantity.ToString();

            GreenTeaPrice.Text = itemGreenTea.price.ToString();
            GreenTeaQuantity.Text = itemGreenTea.quantity.ToString();

            NoodlePrice.Text = itemNoodle.price.ToString();
            NoodleQuantity.Text = itemNoodle.quantity.ToString();

            PizzaPrice.Text = itemPizza.price.ToString();
            PizzaQuantity.Text = itemPizza.quantity.ToString();

            // Bind events
            Coffee.CheckedChanged += Coffee_CheckedChanged;
            GreenTea.CheckedChanged += GreenTea_CheckedChanged;
            Noodle.CheckedChanged += Noodle_CheckedChanged;
            Pizza.CheckedChanged += Pizza_CheckedChanged;

            CoffeeQuantity.TextChanged += CoffeeQuantity_TextChanged;
            GreenTeaQuantity.TextChanged += GreenTeaQuantity_TextChanged;
            NoodleQuantity.TextChanged += NoodleQuantity_TextChanged;
            PizzaQuantity.TextChanged += PizzaQuantity_TextChanged;
        }

        private void Checkout_Click(object sender, EventArgs e)
        {
            double total = getPriceFromSelectedItems();
            tbTotal.Text = total.ToString("F2");
            calculateChange(total);
        }

        private double getPriceFromSelectedItems()
        {
            double total = 0;

            // Calculate item totals
            if (itemCoffee.isCheck)
                total += calculateItemTotal(itemCoffee, CoffeeQuantity.Text);

            if (itemGreenTea.isCheck)
                total += calculateItemTotal(itemGreenTea, GreenTeaQuantity.Text);

            if (itemNoodle.isCheck)
                total += calculateItemTotal(itemNoodle, NoodleQuantity.Text);

            if (itemPizza.isCheck)
                total += calculateItemTotal(itemPizza, PizzaQuantity.Text);

            // Apply discounts
            if (All.Checked)
            {
                float discountRate = float.TryParse(textBox1.Text, out float rateAll) ? rateAll : 0;
                total -= total * (discountRate / 100);
            }
            else if (Beverage.Checked)
            {
                float beverageTotal = (itemCoffee.isCheck ? itemCoffee.price * itemCoffee.quantity : 0) +
                                      (itemGreenTea.isCheck ? itemGreenTea.price * itemGreenTea.quantity : 0);
                float discountRate = float.TryParse(textBox2.Text, out float rateBeverage) ? rateBeverage : 0;
                total -= beverageTotal * (discountRate / 100);
            }
            else if (Food.Checked)
            {
                float foodTotal = (itemNoodle.isCheck ? itemNoodle.price * itemNoodle.quantity : 0) +
                                  (itemPizza.isCheck ? itemPizza.price * itemPizza.quantity : 0);
                float discountRate = float.TryParse(textBox3.Text, out float rateFood) ? rateFood : 0;
                total -= foodTotal * (discountRate / 100);
            }

            return total;
        }

        private double calculateItemTotal(Item item, string quantityText)
        {
            if (int.TryParse(quantityText, out int quantity))
            {
                item.quantity = quantity;
                return item.price * item.quantity;
            }
            else
            {
                item.quantity = 0;
                return 0;
            }
        }

        private void calculateChange(double total)
        {
            if (double.TryParse(tbCash.Text, out double cashReceived))
            {
                if (cashReceived >= total)
                {
                    double change = cashReceived - total;
                    tbChange.Text = change.ToString("F2");

                    // Calculate change in denominations
                    int[] denominations = { 1000, 500, 100, 50, 20, 10, 5, 1 };
                    double remainingChange = change;

                    txt1000.Text = txt500.Text = txt100.Text = txt50.Text =
                        txt20.Text = txt10.Text = txt5.Text = txt1.Text = "";

                    foreach (int denomination in denominations)
                    {
                        int count = (int)(remainingChange / denomination);
                        remainingChange %= denomination;

                        switch (denomination)
                        {
                            case 1000: txt1000.Text = count.ToString(); break;
                            case 500: txt500.Text = count.ToString(); break;
                            case 100: txt100.Text = count.ToString(); break;
                            case 50: txt50.Text = count.ToString(); break;
                            case 20: txt20.Text = count.ToString(); break;
                            case 10: txt10.Text = count.ToString(); break;
                            case 5: txt5.Text = count.ToString(); break;
                            case 1: txt1.Text = count.ToString(); break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("เงินที่ได้รับน้อยกว่าราคาสินค้า!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("กรุณากรอกจำนวนเงินสดที่ถูกต้อง!", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Event handlers for checkboxes
        private void Coffee_CheckedChanged(object sender, EventArgs e) => itemCoffee.isCheck = Coffee.Checked;
        private void GreenTea_CheckedChanged(object sender, EventArgs e) => itemGreenTea.isCheck = GreenTea.Checked;
        private void Noodle_CheckedChanged(object sender, EventArgs e) => itemNoodle.isCheck = Noodle.Checked;
        private void Pizza_CheckedChanged(object sender, EventArgs e) => itemPizza.isCheck = Pizza.Checked;

        // Event handlers for quantity textboxes
        private void CoffeeQuantity_TextChanged(object sender, EventArgs e) => itemCoffee.quantity = parseQuantity(CoffeeQuantity.Text);
        private void GreenTeaQuantity_TextChanged(object sender, EventArgs e) => itemGreenTea.quantity = parseQuantity(GreenTeaQuantity.Text);
        private void NoodleQuantity_TextChanged(object sender, EventArgs e) => itemNoodle.quantity = parseQuantity(NoodleQuantity.Text);
        private void PizzaQuantity_TextChanged(object sender, EventArgs e) => itemPizza.quantity = parseQuantity(PizzaQuantity.Text);

        private int parseQuantity(string text) => int.TryParse(text, out int quantity) ? quantity : 0;
    }

    public class Item
    {
        public string name { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
        public bool isCheck { get; set; }
    }
}
