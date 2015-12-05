using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GladiatorTest
{
    class Program
    {
        static void Main(string[] args)
        {
            char pressanykey;
            double a = 80, b = 6, c = 60;
            double a2 = 98, b2 = 8, c2 = 0;
            int d = 2;
            //Gladiator enemy = new Gladiator(d);
            Gladiator enemy = new Gladiator(a2, b2, c2);
            Gladiator player = new Gladiator(a, b, c);
            bool result;
            string resultmsg = "Empty", log = "empty";
            result = player.battle(enemy);
            log = player.GetLog();
            if (result)
                resultmsg = "You win!";
            else
                resultmsg = "You lose!";
            Console.WriteLine(log + resultmsg);
            pressanykey = (char)Console.Read();
        }
    }
    public class Gladiator
    {
        private double hp;//hit points
        private double ap;//attack points
        private double crit;//critical strike chance bonus
        private double block = 0.3;//block chance
        private double miss = 0.05;//chance to miss
        private double ev = 0.05;//chance evade
        private double xcrit = 3;//Xcrit times critical strike damage is increased
        private double xblock = 0.6;//Amount of blocked damage reduced to Xblock times of normal damage
        private double critcoef = 0.98;//Each N next critical strike point adds (critcoef)^N critical strike chance bonus
        private double maxcritbonus = 0.7;// Max critical strike chance bonus (MCSCB)
        private double basecrit = 0.1;// Base critical strike chance  (BCSC) + MCSCB = Max allowed critical strike chance
        private double basehp = 20;// Base hit points
        private double baseap = 5;// Base attack points
        private string log = "";// Fight log

        public Gladiator()
        {
            hp = basehp;
            ap = baseap;
            crit = basecrit;
        }
        public Gladiator(int lvl)
        {
            switch (lvl)
            {
                case 0:
                    hp = basehp;
                    ap = baseap;
                    crit = basecrit;
                    break;
                case 1:
                    hp = 1.5 * basehp;
                    ap = baseap;
                    crit = basecrit;
                    break;
                case 2:
                    hp = 1.5 * basehp;
                    ap = 1.2 * baseap;
                    crit = 2 * basecrit;
                    break;
                case 3:
                    hp = 3 * basehp;
                    ap = 1.8 * baseap;
                    crit = 2 * basecrit;
                    break;
                default://Like lvl 0
                    hp = basehp;
                    ap = baseap;
                    crit = basecrit;
                    break;
            }
        }
        public Gladiator(double a, double b, double c)
        {
            double critbonus = 0;
            for (int i = 0; i < c; i++)
            {
                critbonus = critbonus + Math.Pow(critcoef, i) / 100;//Formula for critical strike chance bonus
                if (critbonus > maxcritbonus) break;//max allowed crit chance is 0.8
            }
            hp = a + basehp;
            ap = b + baseap;
            crit = basecrit + critbonus;
            //Console.WriteLine("My crit chance={0:0.00}", crit);
        }
        public bool battle(Gladiator enemy)
        {
            Random rnd = new Random();
            double tmpap = ap, tmphp = hp;
            double tmpape = enemy.ap, tmphpe = enemy.hp;
            log = string.Format("Before the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe) + Environment.NewLine;
            //Console.WriteLine("Before the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe);
            if ((rnd.Next(0, 100)%2) == 0)//Player.this strikes first
            {
                int i = 0;
                log += "Player strikes first" + Environment.NewLine;
                //Console.WriteLine("Player strikes first");
                //insert while
                while ((tmphp > 0) && (tmphpe > 0))
                {
                    
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= crit * 100) {
                            tmpap = tmpap * xcrit;
                            log += "Player critically hits!" + Environment.NewLine;
                            //Console.WriteLine("Player critically hits!");
                        }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpap = tmpap * xblock;
                                log += "Enemy blocks the attack!" + Environment.NewLine;
                                //Console.WriteLine("Enemy blocks the attack!");
                            }
                            tmphpe = tmphpe - tmpap;
                            log += string.Format("Player hits for {0:0.00}!", tmpap) + Environment.NewLine;
                            //Console.WriteLine("Player hits for {0:0.00}!", tmpap);
                        }else
                            log += "Enemy evaded the attack!" + Environment.NewLine;
                        //Console.WriteLine("Enemy evaded the attack!");
                    } else
                        log += "Player misses the target!" + Environment.NewLine;
                    //Console.WriteLine("Player misses the target!");
                    if (tmphpe <= 0) {
                        log += string.Format("After the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe) + Environment.NewLine;
                        //Console.WriteLine("After the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe);
                        return true;
                    }
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= enemy.crit * 100) {
                            tmpape = tmpape * xcrit;
                            log += "Enemy critically hits!" + Environment.NewLine;
                            //Console.WriteLine("Enemy critically hits!");
                        }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpape = tmpape * xblock;
                                log += "Player blocks the attack!" + Environment.NewLine;
                                //Console.WriteLine("Player blocks the attack!");
                            }
                            tmphp = tmphp - tmpape;
                            log += string.Format("Enemy hits for {0:0.00}!", tmpape) + Environment.NewLine;
                            //Console.WriteLine("Enemy hits for {0:0.00}!", tmpape);
                        } else
                            log += "Player evaded the attack!" + Environment.NewLine;
                        //Console.WriteLine("Player evaded the attack!");
                    } else
                        log += "Enemy misses the target!" + Environment.NewLine;
                    //Console.WriteLine("Enemy misses the target!");
                    if (tmphp <= 0) {
                        log += string.Format("After the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe) + Environment.NewLine;
                        //Console.WriteLine("After the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe);
                        return false;
                    }
                    tmpap = ap;
                    tmpape = enemy.ap;
                    i++;
                    log += string.Format("My hp={0:0.00}, enemy hp={1}, i={2}", tmphp, tmphpe, i) + Environment.NewLine;
                    //Console.WriteLine("My hp={0:0.00}, enemy hp={1}, i={2}", tmphp, tmphpe, i);
                }

            }
            else//Enemy strikes first
            {
                log += "Enemy strikes first" + Environment.NewLine;
                //Console.WriteLine("Enemy strikes first");
                int i = 0;
                while ((tmphp > 0) && (tmphpe > 0))
                {
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= enemy.crit * 100) {
                            tmpape = tmpape * xcrit;
                            log += "Enemy critically hits!" + Environment.NewLine;
                            //Console.WriteLine("Enemy critically hits!");
                        }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpape = tmpape * xblock;
                                log += "Player blocks the attack!" + Environment.NewLine;
                                //Console.WriteLine("Player blocks the attack!");
                            }
                            tmphp = tmphp - tmpape;
                            log += string.Format("Enemy hits for {0:0.00}!", tmpape) + Environment.NewLine;
                            //Console.WriteLine("Enemy hits for {0:0.00}!", tmpape);
                        }
                        else
                            log += "Player evaded the attack!" + Environment.NewLine;
                        //Console.WriteLine("Player evaded the attack!");
                    }
                    else
                        log += "Enemy misses the target!" + Environment.NewLine;
                    //Console.WriteLine("Enemy misses the target!");
                    if (tmphp <= 0) {
                        log += string.Format("After the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe) + Environment.NewLine;
                        //Console.WriteLine("After the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe);
                        return false;
                    }
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= crit * 100) {
                            tmpap = tmpap * xcrit;
                            log += "Player critically hits!" + Environment.NewLine;
                            //Console.WriteLine("Player critically hits!");
                        }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpap = tmpap * xblock;
                                log += "Enemy blocks the attack!" + Environment.NewLine;
                                //Console.WriteLine("Enemy blocks the attack!");
                            }
                            tmphpe = tmphpe - tmpap;
                            log += string.Format("Player hits for {0:0.00}!", tmpap) + Environment.NewLine;
                            //Console.WriteLine("Player hits for {0:0.00}!", tmpap);
                        }
                        else
                            log += "Enemy evaded the attack!" + Environment.NewLine;
                        //Console.WriteLine("Enemy evaded the attack!");
                    }
                    else
                        log += "Player misses the target!" + Environment.NewLine;
                    //Console.WriteLine("Player misses the target!");
                    if (tmphpe <= 0) {
                        log += string.Format("After the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe) + Environment.NewLine;
                        //Console.WriteLine("After the fight: My hp={0:0.00}, enemy hp={1}", tmphp, tmphpe);
                        return true;
                    }
                    tmpap = ap;
                    tmpape = enemy.ap;
                    i++;
                    log += string.Format("My hp={0:0.00}, enemy hp={1}, i={2}", tmphp, tmphpe, i) + Environment.NewLine;
                    //Console.WriteLine("My hp={0:0.00}, enemy hp={1}, i={2}", tmphp, tmphpe, i);
                }
            }
            if (tmphpe <= 0)
            {
                Console.WriteLine("Abnormal return true");
                return true;
            }
            else
            {
                Console.WriteLine("Abnormal return false");
                return false;
            }


        }

        public string GetLog()
        {
            return log;
        }


    }

}
