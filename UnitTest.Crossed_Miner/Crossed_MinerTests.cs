using Crossed_Miner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTest.Crossed_Miner
{
    [TestClass]
    public class Crossed_MinerTests
    {
        [DataRow(1)]
        [DataRow(5)]
        [TestMethod]
        public void SetupViewModel_EventFiresWhenExpected(int numberOfTimesEventIsFired)
        {
            List<string> receivedEvents = new List<string>();
            SetupViewModel setupVM = new SetupViewModel();
            setupVM.SavedSettingsEvent += delegate (object sender, SetupEventArgs e)
            {
                receivedEvents.Add(e.Server); //Adds the number of times we get setup info back, confirming the EventHandling works as intended
            };

            for (int i = 0; i < numberOfTimesEventIsFired; i++)
            {
                setupVM.SaveSettingsCommand.Execute(null); //Invokes the ICommand in SetupViewModel. The linq expression in this ICommand property requires we pass something, but we don't need to, so null is passed
            }

            Assert.AreEqual(numberOfTimesEventIsFired, receivedEvents.Count);
        }

        [TestMethod]
        public void SetupViewModel_EventSavesData()
        {
            MiningConfig initialConfig = new MiningConfig() { Server = "InitialServer", WalletID = "InitialWalletID", Worker = "InitialWorker" };
            MiningConfig eventConfig = new MiningConfig(); //Takes what the event spits back and holds on to it
            MiningConfig newConfig = new MiningConfig() { Server = "DifferentServer", WalletID = "DifferentWalletID", Worker = "DifferentWorker" };

            SetupViewModel setupVM = new SetupViewModel();
            setupVM.SavedSettingsEvent += delegate (object sender, SetupEventArgs e)
            {
                eventConfig.Server = e.Server;
                eventConfig.Worker = e.Worker;
                eventConfig.WalletID = e.WalletID;
            };

            //Tell the setup window to start with initial values
            setupVM.Initialize(initialConfig);

            //Change up the properties in the SetupViewModel instance - pretends the user entered new data
            setupVM.ChosenServer = newConfig.Server;
            setupVM.Worker = newConfig.Worker;
            setupVM.WalletID = newConfig.WalletID;

            //Pretends the user hit the Save button on the Setup window
            setupVM.SaveSettingsCommand.Execute(null); //Invokes the ICommand in SetupViewModel. The linq expression in this ICommand property requires we pass something, but we don't need to, so null is passed

            //The event fired, which means the eventConfig data should be passed back. Confirm it matches what we set it to
            Assert.AreEqual(newConfig.Server, eventConfig.Server);
            Assert.AreEqual(newConfig.Worker, eventConfig.Worker);
            Assert.AreEqual(newConfig.WalletID, eventConfig.WalletID);
        }

        [TestMethod]
        public void SetupViewModel_EventCancel()
        {
            MiningConfig initialConfig = new MiningConfig() { Server = "InitialServer", WalletID = "InitialWalletID", Worker = "InitialWorker" };
            MiningConfig newConfig = new MiningConfig() { Server = "DifferentServer", WalletID = "DifferentWalletID", Worker = "DifferentWorker" };

            SetupViewModel setupVM = new SetupViewModel();
            setupVM.CancelSettingsEvent += delegate (object sender, EventArgs e)
            {
                setupVM.Initialize(initialConfig);
            };

            //Tell the setup window to start with initial values
            setupVM.Initialize(initialConfig);

            //Change up the properties in the SetupViewModel instance - pretends the user entered new data
            setupVM.ChosenServer = newConfig.Server;
            setupVM.Worker = newConfig.Worker;
            setupVM.WalletID = newConfig.WalletID;

            setupVM.CancelSettingsCommand.Execute(null); //Invokes the ICommand in SetupViewModel. The linq expression in this ICommand property requires we pass something, but we don't need to, so null is passed

            //The event fired, but we didn't save the data. Confirm that the newConfig doesn't match what the setup still has
            Assert.AreEqual(initialConfig.Server, setupVM.ChosenServer);
            Assert.AreEqual(initialConfig.Worker, setupVM.Worker);
            Assert.AreEqual(initialConfig.WalletID, setupVM.WalletID);
        }
    }
}
