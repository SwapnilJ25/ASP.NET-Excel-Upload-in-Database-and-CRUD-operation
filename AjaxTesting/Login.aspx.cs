using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;



public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    [WebMethod]
    public static bool Auth(string loginid, string pass)
    {
        _Auth_user obj = new _Auth_user();
        bool is_Data_Correct = obj.Logindata(loginid, pass);



        if (is_Data_Correct == true)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("This is where you should specify your secret key, which is used to sign and verify Jwt tokens.");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("loginid", loginid.ToString()),
        new Claim("pass", pass) }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            HttpContext.Current.Session["token"] = tokenHandler.WriteToken(token);
            //HttpContext.Current.Session["toke"] = "";

            var refreshTokenHandler = new JwtSecurityTokenHandler();
            var refreshExpiryTime = DateTime.UtcNow.AddMinutes(3); // Set the refresh token's expiration time
            var refreshSecurityToken = new JwtSecurityToken(
                issuer: "your-issuer",
                claims: new[] { new Claim("sub", "user-123") }, // Include user-related claims
                expires: refreshExpiryTime
                
            );
            HttpContext.Current.Session["refreshToken"] = refreshTokenHandler.WriteToken(refreshSecurityToken);

            return  true;
        }
        else
        {
            return false;
        }




    }
    
}
