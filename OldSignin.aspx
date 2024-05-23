<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OldSignin.aspx.vb" Inherits="OldSignin" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <title>SyteLine 8.03.11</title>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <%--<meta name="viewport" content="width=device-width, initial-scale=1">--%>
    <%--<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">--%>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

     <!-- Bootstrap Core CSS -->
    <link href="bower_components/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- MetisMenu CSS -->
    <link href="bower_components/metisMenu/dist/metisMenu.min.css" rel="stylesheet">
    <!-- Custom CSS -->
    <link href="dist/css/sb-admin-2.css" rel="stylesheet">
    <!-- Custom Fonts -->
    <link href="bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">

</head>
<body>
    <form id="form1" runat="server">
    <div>


     <div class="container">
        <div class="row">
            <div class="col-md-4 col-md-offset-4">
                <div class="login-panel panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Please Sign In</h3>
                    </div>
                    <div class="panel-body">
                            <fieldset>
                                <div class="form-group" style="text-align:center" >
                                   <img src="image/TAKLOGO_NEW.png"> 
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="UserNameTextBox" placeholder="User Name" runat="server" CssClass="form-control" autofocus></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="PasswordTextBox" placeholder="Password" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                </div>
                                 <div class="form-group">
                                        <asp:DropDownList ID="ConfigurationDropDown" runat="server" placeholder="Configuration" CssClass="form-control">
                                        </asp:DropDownList>
                                </div>

                                <div class="form-group">
                                    <div class="checkbox">
                                        <label>
                                            <asp:CheckBox ID="RememberCheckBox" runat="server" />Remember Me
                                        </label>
                                    </div>
                                </div>

                                <asp:Button ID="SignInButton" runat="server" Text="Sign In" CssClass="btn btn-lg btn-success btn-block" />
                            </fieldset>
                    </div>
                </div>
            </div>
        </div>
         <asp:Panel ID="NotificationPanel" runat="server" CssClass= "alert alert-danger alert-dismissable" Visible="false">
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
         </asp:Panel>

    </div>

    </div>
    </form>

    <!-- jQuery -->
    <script src="bower_components/jquery/dist/jquery.min.js" type="text/javascript"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js" type="text/javascript"></script>

    <!-- Metis Menu Plugin JavaScript -->
    <script src="bower_components/metisMenu/dist/metisMenu.min.js" type="text/javascript"></script>

    <!-- Custom Theme JavaScript -->
    <script src="dist/js/sb-admin-2.js" type="text/javascript"></script>

    <!-- Page-Level Demo Scripts - Notifications - Use for reference -->
    <script type="text/javascript">
        // tooltip demo
        $('.tooltip-demo').tooltip({
            selector: "[data-toggle=tooltip]",
            container: "body"
        })

        // popover demo
        $("[data-toggle=popover]")
        .popover()
    </script>

</body>
</html>

