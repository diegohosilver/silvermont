<%@ Page Title="" Language="C#" MasterPageFile="~/SilverMont.master" AutoEventWireup="true" CodeFile="Bienvenido.aspx.cs" Inherits="Inicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script>
        $(window).load(function () {
                $("html, body").animate({ scrollTop: 105 }, 700);
        });
    </script>          
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">		
    <!-- Banner. Solo para Bienvenida! -->
		<div id="banner-wrapper">
            <section id="banner" style="z-index:1;">    
		    	<h2>Bienvenido a SilverMont</h2>
				<p>Toda la info de tus series favoritas, en un solo lugar</p>
				<a href="Inicio.aspx" class="button">Ok, comencemos!</a>
			</section>
        </div>		
</asp:Content>

