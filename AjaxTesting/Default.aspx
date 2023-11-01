<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" ClientIDMode="Static" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.14.7/dist/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.1/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#btn_Add').click(function () {
                var name = $("#name").val();
                var pass = $("#pass").val();
                var age = $("#age").val();
                var id = $(this).attr("data-id")
                if (name.length == 0 || pass.length == 0 || age.length == 0) { alert("Enter All Value....."); }
                else {
                    console.log("name " + name);
                    console.log("pass " + pass);
                    console.log("age " + age);
                    //mydata = { name: "name", pass: "pass", age: "age" }
                    //mydata = { "name": name, "pass": pass, "age": age };
                    mydata = { name: "" + name + "", pass: "" + pass + "", age: "" + age + "", id: "" + id + "" }


                    $.ajax({
                        global: false,
                        url: "Default.aspx/Add",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: JSON.stringify(mydata),
                        success: function (response, textStatus, xhr) {
                            alert(response.d);
                            if (response.d == "token expired") {
                                window.location.href = "Login.aspx";
                            }
                            else
                            {
                                console.log("response.d", response.d);
                                console.log("success" + response);
                                $("#name").val('');
                                $("#pass").val('');
                                $("#age").val('');
                            }

                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(errorThrown);
                        }
                    });
                    return false;
                }
            });





            $('.btn_del_del').click(function () {
                var id = $(this).attr("data-id")
                alert("In delete")

                console.log("------>  ", id)
                console.log("------>  ", typeof (id))
                id = parseInt(id, 10);
                console.log("------>  ", typeof (id))

                //mydata = { name: "name", pass: "pass", age: "age" }
                //mydata = { "name": name, "pass": pass, "age": age };
                mydata = {  id: "" + id + "" }

                $.ajaxSetup({
                    global: false,
                    url: "Default.aspx/Del",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    /*headers: {
                        '__RequestVerificationToken': $('#hddn_CSRF').val()
                    },*/


                    //data: JSON.stringify({ name: "name", pass: "pass", age: "age" }),


                    beforeSend: function () {
                        console.log("beforeSend");
                        //var block_ele = $('body');
                        //block_ele.block({
                        //    message: '<div class="fa fa-spinner fa-spin" style="font-size: 30px;"></div><br/><h4>Please Wait...!</h4>',
                        //    overlayCSS: {
                        //        backgroundColor: '#FFF',
                        //        cursor: 'wait',
                        //    },
                        //    css: {
                        //        border: 0,
                        //        padding: 0,
                        //        backgroundColor: 'none'
                        //    }
                        //});

                        //$('body').css('overflow', 'hidden');
                    },
                    complete: function () {
                        console.log("complete");
                        //$('body').find('div.blockUI').remove();
                        //$('body').css('overflow', 'auto');
                    }
                });

                $.ajax({
                    data: JSON.stringify(mydata),//"{name:'" + $("#name").val() + "', pass:'" + $("#pass").val() +"', age:'" + $("#age").val() +"'}",
                    success: function (response, textStatus, xhr) {
                        alert(response.d);


                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown);
                    }
                });
                return false;
                
            });


           
            $(".updbtn").click(function () {

                var id = $(this).attr("data-id")
                mydata = { id: "" + id + "" }
                console.log(id)

                $.ajaxSetup({
                    global: false,
                    url: "Default.aspx/Update",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    /*headers: {
                        '__RequestVerificationToken': $('#hddn_CSRF').val()
                    },*/


                    //data: JSON.stringify({ name: "name", pass: "pass", age: "age" }),


                    beforeSend: function () {
                        console.log("beforeSend");
                        //var block_ele = $('body');
                        //block_ele.block({
                        //    message: '<div class="fa fa-spinner fa-spin" style="font-size: 30px;"></div><br/><h4>Please Wait...!</h4>',
                        //    overlayCSS: {
                        //        backgroundColor: '#FFF',
                        //        cursor: 'wait',
                        //    },
                        //    css: {
                        //        border: 0,
                        //        padding: 0,
                        //        backgroundColor: 'none'
                        //    }
                        //});

                        //$('body').css('overflow', 'hidden');
                    },
                    complete: function () {
                        console.log("complete");
                        //$('body').find('div.blockUI').remove();
                        //$('body').css('overflow', 'auto');
                    }
                });

                $.ajax({
                    data: JSON.stringify(mydata),//"{name:'" + $("#name").val() + "', pass:'" + $("#pass").val() +"', age:'" + $("#age").val() +"'}",
                    success: function (response, textStatus, xhr) {
                        //alert(response.d);
                        console.log("success")
                        
                        var data = JSON.parse(response.d);
                        var newPasswordInput = $("<input>").attr({
                            "type": "text",
                            "class": "form-control",
                            "id": "pass",
                            "placeholder": "Password"
                        });

                        // Copy the password value from the old input to the new input
                        newPasswordInput.val($("#pass").val());
                        $("#pass").replaceWith(newPasswordInput);
                        console.log("-->  ", data[0])
                        $("#name").val(data[0]['Name'])
                        
                        $("#pass").val(data[0]['Pass'])
                        $("#age").val(data[0]['Age'])
                        var button = $("#btn_Add");

                        // Change the data-id attribute value to 2
                        button.attr("data-id", data[0]['id'] );

                        // Change the button text from "Insert" to "Update"
                        button.text("Update");
          

                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(errorThrown);
                    }
                });
                return false;
            });

  

        });
    </script>

</head>
<body style="background-color:#F3E39A">



    <nav class="navbar navbar-expand-lg navbar-dark d-none d-lg-block" style='background: darkgray ' >
    <div class="container-fluid">
      <!-- Navbar brand -->
      <a class="navbar-brand nav-link" href="/#">
        <strong style="color:white">Web Form</strong>
      </a>
      <div class="collapse navbar-collapse" id="navbarExample01">
        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
          <li class="nav-item active">
            <a class="btn btn-outline-dark" type="submit" style="color:white" href="#">CRUD</a>
          </li>

         
        </ul>
      </div>

      <a type="submit" class="btn btn-outline-dark me-3" href="/Login.aspx" style="color:white">Logout</a>
    </div>
  </nav>


    <form class="m-3" runat="server">
        <div class="form-row">
            <div class="form-group col-md-4 mb-2">
                <label for="inputEmail4">Name :-</label>
                <input type="text" class="form-control" id="name" placeholder="Enter Name..">
            </div>
            <div class="form-group col-md-4 mb-2">
                <label for="inputPassword4">Password :-</label>
                <input type="password" class="form-control" id="pass" placeholder="Password">
            </div>
        </div>
        <div class="form-group col-md-4 mb-2">
            <label for="inputAddress">Age :-</label>
            <input type="number" class="form-control" id="age" placeholder="Enter Your Age">
        </div>
        <button type="button" class="btn btn-primary mr-2 mt-1" data-id=-45 id="btn_Add">Insert</button>


        <div id="showdata" runat="server" >
            <%=getdate()%>
        </div>
    </form>


    
</body>
</html>
