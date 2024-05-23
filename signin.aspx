<%@ Page Language="VB" AutoEventWireup="false" CodeFile="signin.aspx.vb" Inherits="signin" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <title>TAK_WMS</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="icon" href="image/TAK-01.png" sizes="32x32" type="image/png" />
    <link rel="icon" href="image/TAK-01.png" sizes="16x16" type="image/png" />
    <link href="bootstrap/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/font-awesome.css" rel="stylesheet" type="text/css" />
    <link href="bootstrap/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="asset/Signin/bootstrap.min.css" rel="stylesheet" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous" type="text/css" />
    <%--<link rel="apple-touch-icon" href="~/image/TAKLOGO_NEW.png" sizes="180x180" />--%>
    <link rel="manifest" href="asset/Signin/manifest.json" />
    <link rel="mask-icon" href="asset/Signin/safari-pinned-tab.svg" color="#563d7c" />
    <meta name="msapplication-config" content="asset/Signin/browserconfig.xml" />
    <meta name="theme-color" content="#563d7c" />
    <script src="asset/Signin/sweetalert2.min.js"></script>
    <link href="asset/Signin/sweetalert2.min.css" rel="stylesheet" type="text/css" />
    <script>

        function ShowSweetAlert(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            )

        }

    </script>

    <style type="text/css">
        .button,
        .button:hover {
            background-color: #0f288d;
            color: #fff;
            border-radius: 15px;
        }
    </style>

    <script type="text/javascript">

        function ShowSweetAlertWarning(type, msg, icon) {
            Swal.fire(
                type,
                msg,
                icon
            ).then((result) => {  
 
                if (result.value) {
                    window.location = "Menu.aspx";
 	            }
            });                  

        }

     </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">

        <br />
        <div class="row">

            <div class="col-sm-3"></div>

            <div class="col-sm-6 pt-5">
                <div class="shadow p-3 mb-5 text-center border-primary p-5" style="border-radius:25px; background-color:#fff;">
                        <span>
                            <img src="image/TAK-01.png" alt="TAK">
                        </span>
                        <div class="input-group my-4">

                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-user"></i></span>
                            </div>

                            <asp:TextBox ID="UserNameTextBox" runat="server" class="form-control" placeholder="Username"></asp:TextBox>
                        </div>

                        
                    <div class="input-group mb-4">

                            <div class="input-group-prepend">
                                <span class="input-group-text"><i class="fa fa-lock"></i></span>
                            </div>

                            <asp:TextBox ID="PasswordTextBox" runat="server" class="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>

                    </div>

                    <asp:DropDownList ID="ConfigurationDropDown" runat="server" class="form-control"></asp:DropDownList>

                    <asp:Button ID="SignInButton" runat="server" Text="Login" class="btn btn-block button my-4" />

                </div>
		    </div>

            <div class="col-sm-3"></div>

        </div>
 
    </div>
    </form>

</body>
</html>
