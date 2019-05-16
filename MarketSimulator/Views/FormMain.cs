using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using MarketSimulator.Models;
using System.Threading;
using System.Reflection;

namespace MarketSimulator
{
    public partial class FormMain : Form
    {

        List<Task> threads = new List<Task>(); // list de task
        Thread runningTaskThread;  // variavel que recebe  a thread em funcionamento

        public FormMain()
        {
            InitializeComponent(); // inica adicionando tasks
            threads.Add(new Task(() => ThreadStack(pnlClienteCx1, picComprasCx1, lblTotalCx1, lblRestentesCx1, 0, lblPowerCx1, progressBarFila)));
            threads.Add(new Task(() => ThreadStack(pnlClienteCx2, picComprasCx2, lblTotalCx2, lblRestentesCx2, 1, lblPowerCx2, progressBarFila)));
            threads.Add(new Task(() => ThreadStack(pnlClienteCx3, picComprasCx3, lblTotalCx3, lblRestentesCx3, 2, lblPowerCx3, progressBarFila)));
            threads.Add(new Task(() => ThreadStack(pnlClienteCx4, picComprasCx4, lblTotalCx4, lblRestentesCx4, 3, lblPowerCx4, progressBarFila)));
            threads.Add(new Task(() => ThreadStack(pnlClienteCx5, picComprasCx5, lblTotalCx5, lblRestentesCx5, 4, lblPowerCx5, progressBarFila)));
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
                foreach (Task t in threads)
                {
                    await Task.Delay(rand.Next(1, 400));
                    t.Start();
                }
                btnStart.Enabled = false;
            }
            catch { Application.Restart(); } //caso occora alguma erro a aplicaçao reinicia
        }

        private async void ThreadStack(Panel pnlCliente, PictureBox picProduct, Label lblTotal, Label lblRestante, int index, Label lblPower, ProgressBar progress)
        {
            try
            {
                while (true)   // metodo que faz o controle visual do que ocorre no mercado
                {

                    Random rand = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

                    int idleTime = rand.Next(1, 10000);

                    await Task.Delay(idleTime); //atrasa a execução da tarefa

                    MyQueue<Customer> customers = new MyQueue<Customer>(); // fila de clientes para cada caixa

                    for (int i = 0; i < rand.Next(2, 4); i++)
                    {

                        customers.Enqueue(new Customer());

                    }

                    int proFila = progress.Value + customers.Count() - 1;
                    SetControlPropertyValue(progress, "Value", proFila);

                    while (customers.Count() != 1)
                    {
                        runningTaskThread = Thread.CurrentThread;
                        Customer customer = customers.Peek();

                        SetControlPropertyValue(pnlCliente, "BackgroundImage", customer.CustomerPic);
                        SetControlPropertyValue(picProduct, "Image", customer.ProductsPic);
                        SetControlPropertyValue(lblTotal, "Text", "Total: " + customer.Purchase.Count.ToString());
                        SetControlPropertyValue(lblRestante, "Text", "Restantes: " + customer.Purchase.Count.ToString());
                        SetControlPropertyValue(lblPower, "ForeColor", Color.Lime);

                        for (int i = customer.Purchase.Count - 1; i >= 0; i--)
                        {
                            await Task.Delay(customer.Purchase[i].ProductTime);
                            customer.Purchase.RemoveAt(i);
                            SetControlPropertyValue(lblRestante, "Text", "Restantes: " + i.ToString());
                        }

                        SetControlPropertyValue(picProduct, "Image", customer.PaymentPic);
                        await Task.Delay(customer.PaymentTime);

                        SetControlPropertyValue(picProduct, "Image", null);
                        SetControlPropertyValue(pnlCliente, "BackgroundImage", null);
                        SetControlPropertyValue(lblTotal, "Text", "Total: ");
                        SetControlPropertyValue(lblRestante, "Text", "Restantes: ");
                        customers.Dequeue();
                        proFila = progressBarFila.Value - 1;
                        SetControlPropertyValue(progress, "Value", proFila);

                        Application.DoEvents();
                    }
                }
            }
            catch { return; }
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        //Um delegate é um elemento da linguagem C# que permite que você faça referência a um método.
        private void SetControlPropertyValue(Control oControl, string propName, object propValue) // controle entre tasks ou threads para cada permitir uma alteração por outra thread
        {   // pode ser utilizado  para qualquer windows form basta modificar os controles 
            SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);

            if (oControl.InvokeRequired) oControl.Invoke(d, new object[] { oControl, propName, propValue });
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();

                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                        break;
                    }
                }
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                runningTaskThread.Abort();  // ao sair da application finaliza threads em execução
            }
            catch { }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                runningTaskThread.Abort();
                btnStart.Enabled = true;
            }
            catch { }
        }
    }

}


