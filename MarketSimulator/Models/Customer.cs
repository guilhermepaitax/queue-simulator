using MarketSimulator.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;


namespace MarketSimulator.Models
{
    class Customer  //classe onde é feito o controle de resources images e forma de pagamentos
    {
        private List<Product> purchase = new List<Product>();
        private int paymentTime;
        private Image customerPic;
        private Image paymentPic;
        private Image productsPic;

        public Customer()
        {
            Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            
            this.paymentTime = rand.Next(3000, 7001);
            int custPic = rand.Next(1, 6);
            int payPic = rand.Next(1, 3);
            int prodPic = rand.Next(1, 6);

            if (payPic == 1) paymentPic = Resources.icon_pagamento1;
            else paymentPic = Resources.icon_pagamento2;

            if (custPic == 1) customerPic = Resources.icon_cliente1;
            else if(custPic == 2) customerPic = Resources.icon_cliente2;
            else if (custPic == 3) customerPic = Resources.icon_cliente3;
            else if (custPic == 4) customerPic = Resources.icon_cliente4;
            else customerPic = Resources.icon_cliente5;

            if (prodPic == 1) productsPic = Resources.icon_cesta_azul;
            else if (prodPic == 2) productsPic = Resources.icon_cesta_papel;
            else if (prodPic == 3) productsPic = Resources.icon_cesta_rosa;
            else if (prodPic == 4) productsPic = Resources.icon_cesta_verde;
            else productsPic = Resources.icon_cesta_vermelha;

            int productsCount = rand.Next(2,101);

            for (int i = 1; i < productsCount; i++)
            {
                purchase.Add(new Product());
            }

        }

        public int PaymentTime { get => paymentTime; set => paymentTime = value; }
        public Image CustomerPic { get => customerPic; set => customerPic = value; }
        public Image PaymentPic { get => paymentPic; set => paymentPic = value; }
        public Image ProductsPic { get => productsPic; set => productsPic = value; }
        internal List<Product> Purchase { get => purchase; set => purchase = value; }
    }
}
