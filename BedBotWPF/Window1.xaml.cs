using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tibia;
using Tibia.Constants;
using Tibia.Objects;
using Tibia.Util;
using Tibia.Packets;
using Tibia.Packets.Outgoing;
using Tibia.Packets.Incoming;
using LuaInterface;

namespace BedBotWPF
{

    public partial class Window1 : Window
    {
        bool added = false;
        Client client;
        Player player;
        bool added = false;
        Channel chan = new Channel(ChatChannel.Custom, "BedBot");
        public Window1()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            client = Tibia.Util.ClientChooser.ShowBox();
            if (client != null)
            {
                client.StartProxy();
                client.InitializePipe();
                client.ContextMenu.Click += new Tibia.Objects.ContextMenu.ContextMenuEvent(ContextMenu_Click);
                client.Proxy.OnLogIn += (Proxy.ProxyNotification)OnLogIn;
                ChannelOpenPacket.Send(client, chan);
            }
            else
            {
                MessageBox.Show("Please select or start a new Tibia client.", "Error");
                Application.Exit();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!added)
            {
                client.ContextMenu.AddContextMenu(1000, "Item Number", Tibia.Constants.ContextMenuType.TradeWithContextMenu, true);
                added = true;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (added)
            {
                client.ContextMenu.RemoveContextMenu(1000, "Item Number", Tibia.Constants.ContextMenuType.TradeWithContextMenu, true);
                added = false;
            }
        }
        private void ContextMenu_Click(int eventId)
        {
            if (eventId == 1000)
            {
                Tibia.Packets.Incoming.TextMessagePacket.Send(client, Tibia.Packets.StatusMessage.StatusSmall, "Item Number: " + client.ReadUInt32(Tibia.Addresses.Client.ClickContextMenuItem_Ground_Id));
            }
        }
        }
    }