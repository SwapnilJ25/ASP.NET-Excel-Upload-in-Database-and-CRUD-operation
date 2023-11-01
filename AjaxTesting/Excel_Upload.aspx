<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Excel_Upload.aspx.cs" Inherits="Excel_Upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <script
      src="https://code.jquery.com/jquery-3.6.0.min.js"
      integrity="sha256-/xUj+3OJU5yExlq6GSYGSHk7tPXikynS7ogEvDej/m4="
      crossorigin="anonymous"
    ></script>

     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>
     <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous">



</head>
<body>
    <div class="container pt-5">
    <form id="form1" runat="server">
    <div class="mb-3">
    <label>Upload Excel File:</label>
  <input class="form-control" type="file" id="formFile1"  name="filename" accept=".xlsx, .xls">
        <br>
  <button type="button" class="btn btn-primary" id="submit_btn">Submit</button>
</div>
    </form>

        <div id="idDisplay" >

        </div>


        </div>
    
</body>
    <script>
        $(document).ready(function () {

            $('#submit_btn').click(function () {
                //var fileInput = $('#myFile')[0].files[0];
                var fileUpload = $("#formFile1").get(0);
                var files = fileUpload.files;
                console.log(files);

                if (files == null) {
                    alert("please select a file");
                }
                else
                {
                    if (files != null || files != undefined) {
                        var data = new FormData();
                        for (var i = 0; i < files.length; i++) {
                            data.append(files[i].name, files[i]);
                        }
                        $.ajax({
                            url: "Excelfilename.ashx",
                            type: "POST",
                            data: data,
                            contentType: false,
                            processData: false,
                            success: function (result) {
                                if (result == 'Successfully') {
                                    console.log("[[[[[[[[]]]]]]]")
                                    show_text()
                                    //$('#Hiddupdurl').val('');
                                }

                            },
                            error: function (err) {
                                //$('#Hiddupdurl').val('');
                                alert(err.statusText)
                            }
                        });
                    }
                    return false;
                }//Else end

            });

            function show_text() {
                var fileInput = document.getElementById("formFile1");
                var file = fileInput.files[0];
                //console.log("file___", file)
                //let lang = document.getElementsByName('fav_language');
                //let day = $('input[name="name_day"]:checked');
                mydata = { file_name: "" + file.name + "" }


                console.log(mydata)
                $.ajaxSetup({
                    global: false,
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    url: "Excel_Upload.aspx/upload_text",
                    beforeSend: function () {

                    },
                    complete: function () {

                    }
                });
                $.ajax({
                    data: JSON.stringify(mydata),
                    //data:{},
                    success: function (response, textStatus, xhr) {
                       
                       
                        var divElement = document.getElementById("idDisplay");
                        divElement.innerHTML = "<a id='first_link'>Total Rows: " + response.d[0] + "</a><br>" +
                         "<a id='second_link'>Valid Rows: " + response.d[1] + "</a><br>" +
                         "<a id='third_link'>Invalid Rows: " + response.d[2] + "</a>"




                        $('#first_link').click(function () {
                            alert("Hiiiii");
                            mydata = { total_rows_before: "" + response.d[3] + "", total_rows_after: "" + response.d[4] + "" }

                            console.log("=", mydata)
                            $.ajaxSetup({
                                global: false,
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                url: "Excel_Upload.aspx/Get_total_row",
                                beforeSend: function () {

                                },
                                complete: function () {

                                }
                            });
                            $.ajax({
                                data: JSON.stringify(mydata),
                                //data:{},
                                success: function (mydata, textStatus, xhr) {
                                    console.log("#### ", mydata.d)
                                    var binaryData = [];
                                    binaryData.push(mydata.d);
                                    var url = null;
                                    url = window.URL.createObjectURL(new Blob(binaryData, { type: "application/excel" }))
                                    var a = document.createElement('a');
                                    a.href = "\\uploaded_file\\" + mydata.d;
                                    a.download = mydata.d;
                                    a.click();
                                    window.URL.revokeObjectURL(url);

                                },
                            });
                        });



 







                    },
                    //error: function (jqXHR, textStatus, errorThrown) {

                    //}
                });
            }



            



        });
    </script>



</html>
