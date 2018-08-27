<%@ Page Title="" Language="C#" MasterPageFile="~/SilverMont.master" AutoEventWireup="true" CodeFile="NoticiasArchivo.aspx.cs" Inherits="NoticiasArchivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        var owl;
        $(function ($) {
            $("#banner-noticias").owlCarousel({
                items: 1,
                lazyLoad: true,
                loop: true,
                margin: 10,
                autoplay: true,
                autoplayTimeout: 4000,
                autoplayHoverPause: false
            });
            owl = $('#contenido-noticias').owlCarousel({
                items: 1,
                loop: true,
                touchDrag: false,
                mouseDrag: false,
                margin: 10,
                autoHeight: true,
                nav: true,
                navText: ["<input type='button' class='button' id='btnAtras' value='Atras' />", "<input type='button' class='button' id='btnSiguiente' value='Siguiente' />"]
            });
            //Marcar el boton de la pagina como ACTIVO
            var node = document.getElementById("nav").getElementsByTagName("li")[1];
            node.setAttribute("class", "current");
            //refrescar item carrousel
            setInterval(function () { refresh(); }, 1);
        });     
        $(window).load(function () {
            //Hacer scroll unicamente cuando carga una serie
            if (document.getElementById('<%= hdScroll.ClientID%>').value == "1") {
                $("html, body").animate({ scrollTop: 227 }, 700);
            }
        });

        function refresh() {
            owl.trigger('refresh.owl.carousel');
        }
    </script>     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Main -->
	<div id="main-wrapper">
		<div id="main" class="container">
            <div class="row">
            <div class="3u 12u(mobile)">
            <div class="sidebar">
                <!-- Sidebar -->
                <!-- Archives -->
				<ul class="divided">
					<asp:DataList ID="dlNoticias" runat="server" OnItemCommand="dlNoticias_ItemCommand" OnItemDataBound="dlNoticias_ItemDataBound">
                        <ItemTemplate>
                            <li>
								<article class="box post-summary">
                                    <asp:HiddenField ID="hfIDAux" runat="server" Value='<%#Eval("id") %>' />
									<h3><a href="#"><asp:LinkButton ID="lblTitulo" runat="server" Text='<%# Eval("Titulo") %>' CommandName="CargarNoticia" CommandArgument='<%# Eval("id") %>'></asp:LinkButton></a></h3>
									<ul class="meta">
										<li class="icon fa-clock-o"><asp:Label ID="lblTiempoPublicacion" runat="server" Text='<%# Eval("FechaPublicacion") %>'></asp:Label></li>
										<li class="icon fa-comments"><asp:Label ID="lblCantidadComentarios" runat="server" Text="27"></asp:Label></li>
									</ul>
								</article>
							</li>
                            <br />
                        </ItemTemplate>
                    </asp:DataList>
				</ul>
            </div>
                </div>
                <div class="9u 12u(mobile) important(mobile)">
                <div class="content content-right">
                        <!-- Contenido inicial -->
                    <asp:Panel ID="PanelContenidoInicial" runat="server">
                              <article class="box page-content">

								<header>
									<h2>Seccion noticias</h2>
									<p>Primicias e informacion a toda hora. Porque cuando se trata de informarte, sos nuestra prioridad</p>
								</header>
                                        
								<section>
                                    <div id="banner-noticias" class="owl-carousel owl-theme" style="margin-bottom:-50px">
                                      <div> <span class="image featured"><img class="owl-lazy" data-src="images/bryancranston.jpg" alt="" /></span> </div>
                                      <div> <span class="image featured"><img class="owl-lazy" data-src="images/kevinspacey.jpg" alt="" /></span> </div>
                                      <div> <span class="image featured"><img class="owl-lazy" data-src="images/ramimalek.jpg" alt="" /></span> </div>
                                    </div>	
									<p>
										Noticias e informacion sobre actores, series, capitulos, novedades y mucho mas!
									</p>
								</section>

							</article>
                    </asp:Panel>
                    
                    <!-- Info gral de la noticia -->
                    <asp:Panel ID="PanelContenidoNoticia" runat="server" Visible="false">
                                            <!-- Featured Post -->
														<article class="box post">
															<header>
																<h3><a href="#"></a><asp:Label ID="lblTitulo" runat="server"></asp:Label></h3>
																<p><asp:Label ID="lblCopete" runat="server"></asp:Label></p>
																<ul class="meta">
																	<li class="icon fa-clock-o"><asp:Label ID="lblTiempoPublicado" runat="server"></asp:Label></li>
																	<li class="icon fa-comments"><a href="#"><asp:Label ID="lblCantidadComentarios" runat="server"></asp:Label></a></li>
																</ul>
															</header>
															<asp:Image ID="imgNoticia" runat="server" CssClass="image featured"/>
															<p>
                                                                <asp:Label ID="lblContenido" runat="server"></asp:Label>
															</p>
                                                            <asp:HiddenField ID="hdID" runat="server" />
                                                            <asp:Button ID="btnVerMas" runat="server" CssClass="button" Text="Leer mas" OnClick="btnVerMas_Click"/>
														</article>
                               </asp:Panel>
					       </div>
                       </div>
                   </div>            
               </div>
           </div>
<asp:HiddenField ID="hdScroll" runat="server" />
</asp:Content>

