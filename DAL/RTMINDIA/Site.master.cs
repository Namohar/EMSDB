using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["team"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {
            //lblUser.Text = "Welcome: " + Session["user"].ToString().TrimStart('C', 'O', 'R', 'P', '\\');
            string usr = Session["username"].ToString();
            lblUser.Text = Session["username"].ToString().Replace('.', ' ');

            team.Visible = false;
           // OrdersScore.Visible = false;
            orderDesk.Visible = false;
            impl.Visible = false;
            onboard.Visible = false;
            qc.Visible = false;
            opssupport.Visible = false;
            audit.Visible = false;
            ipvDetails.Visible = false;
            cmpDetails.Visible = false;
            internalmetrics.Visible = false;
            catelog.Visible = false;
            call.Visible = false;
            wem.Visible = false;
            ODHourly.Visible = false;
            OrderDetails.Visible = false;
            ipv1.Visible = false;
            cmp1.Visible = false;
            OpsDev.Visible = false;
            //rtStatus.Visible = false;
            clientService1.Visible = false;
            clientService2.Visible = false;
            Account.Visible = false;
            provisioning.Visible = false;
            if (Session["access"].ToString() == "1")
            {
                //update.Visible = false;
                //action.Visible = false;
                //comments.Visible = false;
                team.Visible = true;
                impl.Visible = true;
                onboard.Visible = true;
                qc.Visible = true;
                audit.Visible = true;
                ipvDetails.Visible = true;
                cmpDetails.Visible = true;
                opssupport.Visible = true;
                catelog.Visible = true;
                call.Visible = true;
                wem.Visible = true;
                ODHourly.Visible = true;
                OrderDetails.Visible = true;
                ipv1.Visible = true;
                cmp1.Visible = true;
                OpsDev.Visible = true;
                //rtStatus.Visible = true;
                clientService1.Visible = true;
                clientService2.Visible = true;
                Account.Visible = true;
                provisioning.Visible = true;
            }
            else if (Session["access"].ToString() == "2")
            {
                team.Visible = true;
                impl.Visible = true;
                onboard.Visible = true;
                qc.Visible = true;
                audit.Visible = true;
                ipvDetails.Visible = true;
                cmpDetails.Visible = true;
                //strike.Visible = false;
                inspire.Visible = false;
                // Account.Visible = false;
                opssupport.Visible = false;
                catelog.Visible = true;
                call.Visible = true;
                wem.Visible = true;
                ODHourly.Visible = true;
                OrderDetails.Visible = true;
                ipv1.Visible = true;
                cmp1.Visible = true;
                //rtStatus.Visible = true;
                clientService1.Visible = true;
                clientService2.Visible = true;
                provisioning.Visible = true;
            }
            else if (Session["access"].ToString() == "3")
            {
                //strike.Visible = false;
                //team.Visible = true;
                inspire.Visible = false;
                // Account.Visible = false;
                //ipvDetails.Visible = true;
                //cmpDetails.Visible = true;
               // rtStatus.Visible = true;
            }
            else if (Session["access"].ToString() == "4")
            {
                // masters.Visible = false;
                schedule.Visible = false;
                //strike.Visible = false;
                //comments.Visible = false;
                inspire.Visible = false;
                //Account.Visible = false;
                log.Visible = false;
            }

            if (Session["team"].ToString() == "7" || Session["team"].ToString() == "8")
            {
                orders.Visible = false;
            }
            else if (Session["team"].ToString() == "1")
            {
                cmp.Visible = false;
                ems.Visible = false;
                ipv.Visible = false;
                psl.Visible = false;
            }
            else
            {
                strike.Visible = false;
            }

            if (Session["access"].ToString() == "1")
            {
                strike.Visible = true;
                cmp.Visible = true;
                ems.Visible = true;
                ipv.Visible = true;
                psl.Visible = true;
                orders.Visible = true;
                //OrdersScore.Visible = true;
                orderDesk.Visible = true;
                internalmetrics.Visible = true;
            }

            // Show Team Menu
            if (Session["team"].ToString() == "13")
            {
                team.Visible = true;
                impl.Visible = true;
            }

            if (Session["team"].ToString() == "10")
            {
                team.Visible = true;
                onboard.Visible = true;
            }
            if (Session["team"].ToString() == "18")
            {
                team.Visible = true;
                opssupport.Visible = true;
            }
            if (Session["team"].ToString() == "9")
            {
                team.Visible = true;
                qc.Visible = true;
            }
            if (Session["team"].ToString() == "11")
            {
                team.Visible = true;
                audit.Visible = true;
            }

            if (Session["team"].ToString() == "1")
            {
                //OrdersScore.Visible = true;
                team.Visible = true;
                orderDesk.Visible = true;
                if (Session["access"].ToString() == "3")
                {
                    OrderDetails.Visible = true;
                    ODHourly.Visible = true;
                }
            }
            if (Session["team"].ToString() == "7")
            {
                team.Visible = true;
                cmpDetails.Visible = true;
                if (Session["access"].ToString() == "3")
                {
                    cmp1.Visible = true;
                }
            }
            if (Session["team"].ToString() == "8")
            {
                team.Visible = true;
                ipvDetails.Visible = true;
                if (Session["access"].ToString() == "3")
                {
                    ipv1.Visible = true;
                }
            }
            if (Session["team"].ToString() == "14")
            {
                team.Visible = true;
                catelog.Visible = true;
            }
            if (Session["team"].ToString() == "19")
            {
                team.Visible = true;
                call.Visible = true;
            }
            if (Session["team"].ToString() == "6")
            {
                team.Visible = true;
                wem.Visible = true;
            }
            if (Session["team"].ToString() == "22")
            {
                team.Visible = true;
                clientService1.Visible = true;
            }
            if (Session["team"].ToString() == "23")
            {
                team.Visible = true;
                clientService2.Visible = true;
            }
            if (Session["team"].ToString() == "21")
            {
                team.Visible = true;
                provisioning.Visible = true;
            }
        }
    }
}
