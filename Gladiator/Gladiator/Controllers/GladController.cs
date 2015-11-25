using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;
using Gladiator.Models;

namespace Gladiator.Controllers
{
    public class GladController : ApiController
    {
        [Route("api/fight")]
        [HttpGet]
        public HttpResponseMessage Fight([FromUri]double a, [FromUri]double b, [FromUri]double c, [FromUri]int d)
        {
            Gladiator enemy = new Gladiator(d);
            Gladiator player = new Gladiator(a, b, c);
            bool result;
            result = player.battle(enemy);
            string xml = string.Format("<result><value>{0}</value><broughtToYouBy>Azure API Management - http://azure.microsoft.com/apim/ </broughtToYouBy></result>", result);
            HttpResponseMessage response = Request.CreateResponse();
            response.Content = new StringContent(xml, System.Text.Encoding.UTF8, "application/xml");
            return response;
        }

    }

    public class Gladiator
    {
        private double hp;//hit points
        private double ap;//attack points
        //private float aspd;//attack speed bonus
        private double crit;//critical strike chance bonus
        private double block=0.3;//block chance
        private double miss=0.05;//chance to miss
        private double ev=0.05;//chance evade
        public Gladiator()
        {
            hp = 20;
            ap = 5;
            crit = 0.1;
        }
        public Gladiator(int lvl)
        {
            switch (lvl)
            {
                case 0:
                    hp = 20;
                    ap = 5;
                    crit = 0.1;
                    break;
                case 1:
                    hp = 30;
                    ap = 5;
                    crit = 0.1;
                    break;
                case 2:
                    hp = 30;
                    ap = 6;
                    crit = 0.2;
                    break;
                default://Like lvl 0
                    hp = 20;
                    ap = 5;
                    crit = 0.1;
                    break;
            }
        }
        public Gladiator(double a, double b, double c)
        {
            hp = a + 20;
            ap = b + 5;
            crit = 0.1 + c*0.01;
        }
        public bool battle(Gladiator enemy)
        {
            Random rnd = new Random();
            double tmpap = ap;
            double tmpape = enemy.ap;
            if (rnd.Next(0, 1) == 0)//Player.this strikes first
            {
                //insert while
                while ((hp > 0) && (enemy.hp > 0))
                {


                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= crit * 100) { tmpap = tmpap * 2; }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpap = tmpap / 2;
                            }
                            enemy.hp = enemy.hp - tmpap;
                        }
                    }
                    if (enemy.hp <= 0) return true;
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= enemy.crit * 100) { tmpape = tmpape * 2; }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpape = tmpape / 2;
                            }
                            hp = hp - tmpape;
                        }
                    }
                    if (hp <= 0) return false;
                    tmpap = ap;
                    tmpape = enemy.ap;
                }

            }
            else//Enemy strikes first
            {
                while ((hp > 0) && (enemy.hp > 0))
                {


                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= enemy.crit * 100) { tmpape = tmpape * 2; }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpape = tmpape / 2;
                            }
                            hp = hp - tmpape;
                        }
                    }
                    if (hp <= 0) return false;
                    if (rnd.Next(0, 100) >= miss * 100)
                    {
                        if (rnd.Next(0, 100) <= crit * 100) { tmpap = tmpap * 2; }
                        if (rnd.Next(0, 100) >= ev * 100)
                        {
                            if (rnd.Next(0, 100) <= block * 100)
                            {
                                tmpap = tmpap / 2;
                            }
                            enemy.hp = enemy.hp - tmpap;
                        }
                    }
                    if (enemy.hp <= 0) return true;
                    tmpap = ap;
                    tmpape = enemy.ap;
                }
            }
            return true;

        }


    }
}
