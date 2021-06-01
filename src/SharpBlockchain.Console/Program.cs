using SharpBlockchain.Core;
using System;
using System.Collections.Generic;

// Blockchain Explained Article - https://towardsdatascience.com/blockchain-explained-using-c-implementation-fb60f29b9f07

namespace SharpBlockchain.CommandLine
{
    public class Program
    {
        static void Main(string[] args)
        {
            int proofOfWorkDifficulty = 2;

            if (args.Length == 1) 
                if (!int.TryParse(args[0], out proofOfWorkDifficulty))
                    proofOfWorkDifficulty = 2;

            const string minerAddress = "miner1";
            const string user1Address = "A";
            const string user2Address = "B";
            
            BlockChain blockChain = new BlockChain(proofOfWorkDifficulty: proofOfWorkDifficulty, miningReward: 10);
            
            blockChain.CreateTransaction(new Transaction(user1Address, user2Address, 200));
            blockChain.CreateTransaction(new Transaction(user2Address, user1Address, 10));

            Console.WriteLine("Is valid: {0}", blockChain.IsValidChain());
            Console.WriteLine();
            Console.WriteLine("--------- Start mining ---------");
            
            blockChain.MineBlock(minerAddress);
            
            Console.WriteLine("BALANCE of the miner: {0}", blockChain.GetBalance(minerAddress));
            
            blockChain.CreateTransaction(new Transaction(user1Address, user2Address, 5));
            
            Console.WriteLine();
            Console.WriteLine("--------- Start mining ---------");
            
            blockChain.MineBlock(minerAddress);
            
            Console.WriteLine("BALANCE of the miner: {0}", blockChain.GetBalance(minerAddress));
            Console.WriteLine();
            
            PrintChain(blockChain);
            
            Console.WriteLine();
            Console.WriteLine("Hacking the blockchain...");
            
            blockChain.Chain[1].Transactions = new List<Transaction> { new Transaction(user1Address, minerAddress, 150) };
            
            Console.WriteLine("Is valid: {0}", blockChain.IsValidChain());
            
            Console.ReadKey();
        }

        private static void PrintChain(BlockChain blockChain)
        {
            Console.WriteLine("----------------- Start Blockchain -----------------");
            
            foreach (Block block in blockChain.Chain)
            {
                Console.WriteLine();
                Console.WriteLine("------ Start Block ------");
                Console.WriteLine("Hash: {0}", block.Hash);
                Console.WriteLine("Previous Hash: {0}", block.PreviousHash);
                Console.WriteLine("--- Start Transactions ---");
                
                foreach (Transaction transaction in block.Transactions)
                {
                    Console.WriteLine("From: {0} To {1} Amount {2}", transaction.From, transaction.To, transaction.Amount);
                }
                
                Console.WriteLine("--- End Transactions ---");
                Console.WriteLine("------ End Block ------");
            }

            Console.WriteLine("----------------- End Blockchain -----------------");
        }
    }
}
