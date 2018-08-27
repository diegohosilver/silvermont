<%@ Page Title="" Language="C#" MasterPageFile="~/SilverMont.master" AutoEventWireup="true" CodeFile="Series.aspx.cs" Inherits="Series" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        var owl;
        $(function ($) {
            //Marcar el boton de la pagina como ACTIVO
            try{
                var node = document.getElementById("nav").getElementsByTagName("li")[document.getElementById('<%= hfElementPosition.ClientID%>').value];
                node.setAttribute("class", "current");
            }
            catch (er) {
                alert(er);
            }
            
            $("#banner-series").owlCarousel({
                items: 1,
                lazyLoad: true,
                loop: true,
                margin: 10,
                autoplay: true,
                autoplayTimeout: 4000,
                autoplayHoverPause: false
            });
            owl = $('#contenido-serie').owlCarousel({
                items: 1,
                loop: true,
                touchDrag: false,
                mouseDrag: false,
                margin: 10,
                autoHeight: true,
                nav: true,
                navText: ["<input type='button' class='button' id='btnAtras' value='Atras' />", "<input type='button' class='button' id='btnSiguiente' value='Siguiente' />"]
            });
            $('.collapsible-panels div').hide();
            $('.collapsible-panels a').click(function (e) {
                $(this).parent().next('.collapsible-panels div').slideToggle(350);
                $(this).parent().toggleClass('active');
                e.preventDefault();
            });
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
                <asp:DataList ID="dlSeries" runat="server" OnItemCommand="dlSeries_ItemCommand">
                   <ItemTemplate>
                        <h2 class="major"><span><asp:LinkButton ID="btnNombreSerie" runat="server" Text='<%# Eval("Nombre") %>' CommandName="CargarSerie" CommandArgument='<%# Eval("id") %>'></asp:LinkButton></span></h2>
                            <p>
                        <asp:Label ID="lblReseña" runat="server" Text='<%# Eval("Reseña") %>'></asp:Label>                                            
                            </p>                                          
                    </ItemTemplate> 
                </asp:DataList>
            </div>
                </div>
                <div class="9u 12u(mobile) important(mobile)">
                <div class="content content-right">
                        <!-- Contenido inicial -->
                    <asp:Panel ID="PanelContenidoInicial" runat="server">
                              <article class="box page-content">

								<header>
									<h2>Seccion series</h2>
									<p>Aqui vas a encontrar informacion sobre todas tus series favoritas</p>
								</header>
                                        
								<section>
                                    <div id="banner-series" class="owl-carousel owl-theme" style="margin-bottom:-50px">
                                      <div> <span class="image featured"><img class="owl-lazy" data-src="images/banner.jpg" alt="" /></span> </div>
                                      <div> <span class="image featured"><img class="owl-lazy" data-src="images/banner-2.jpg" alt="" /></span> </div>
                                      <div> <span class="image featured"><img class="owl-lazy" data-src="images/banner-3.jpg" alt="" /></span> </div>
                                    </div>
									
									<p>
										Noticias, detalles e informacion actual sobre todas y cada una de tus series 
                                        favoritas. Además podes compartir con otros usuarios tus experiencias, opiniones
                                        e ideas sobre cada uno de los capitulos. También podrás dejar un puntaje si te gustó
                                        (o no).
									</p>
								</section>

							</article>
                    </asp:Panel>
                    
                    <!-- Info gral de la serie -->
                    <asp:Panel ID="PanelContenidoSerie" runat="server" Visible="false">
                                    <div id="contenido-serie" class="owl-carousel owl-theme">
                                      <div> 
                                          <!-- Nombre, temporadas y reseña -->
                                         <asp:UpdatePanel ID="PanelContenidoGeneral" runat="server">
                                             <ContentTemplate>
                                                <article class="box page-content">
								                    <header>
									                    <h2><asp:Label ID="lblNombreSerie" runat="server"></asp:Label></h2>
									                    <p>Creado por: <asp:Label ID="lblCreador" runat="server"></asp:Label></p>
								                    </header>
								                    <section>
									                  <span class="image featured"><asp:Image ID="img" runat="server" /></span>
                                                          <ul class="divided">
                                                              <li>
                                                                  <article class="box post-summary">
                                                                      <h3>
                                                                        Cantidad de temporadas: <asp:Label ID="lblTemporadas" runat="server"></asp:Label><br /><br />
                                                                        Género: <asp:Label ID="lblGenero" runat="server"></asp:Label><br /><br />
                                                                        País: <asp:Label ID="lblPais" runat="server"></asp:Label><br /><br />
                                                                        Fecha de estreno: <asp:Label ID="lblAño" runat="server"></asp:Label>
                                                                      </h3>
                                                                  </article>
                                                              </li>
                                                          </ul>
									                    <p>
										                  <asp:Label ID="lblReseña" runat="server"></asp:Label>
									                    </p>
								                   </section>                                                       
                                                </article>
                                            </ContentTemplate>
                                         </asp:UpdatePanel>
                                      </div>
                                        <!-- Temporadas -->
                                      <div>
                                          <asp:UpdatePanel ID="PanelTemporadas" runat="server">
                                              <ContentTemplate>
                                                  <asp:DataList ID="dlTemporadas" runat="server" OnItemCommand="dlTemporadas_ItemCommand" OnItemDataBound="dlTemporadas_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="collapsible-panels">
                                                        <h2 class="nombreCap"><asp:LinkButton ID="btnNroTemporada" runat="server" Text='<%# "Temporada " + Eval("Temporada") %>' CommandName="CargarCapitulosTemporada" CommandArgument='<%# Eval("Temporada") %>'></asp:LinkButton></h2>                                       
                                                        <div>
                                                            <asp:DataList ID="dlCapitulos" runat="server" OnItemCommand="dlCapitulos_ItemCommand">
                                                            <ItemTemplate>
                                                                <h2 class="nombreCap"><asp:LinkButton ID="btnNombreCapitulo" runat="server" Text='<%# Eval("Temporada") + "x" + Eval("Numero") + " - " + Eval("Nombre") %>' CommandName="CargarCapitulo" CommandArgument='<%# Eval("id") %>'></asp:LinkButton></h2>                                       
                                                            </ItemTemplate>                                              
                                                       </asp:DataList>
                                                        </div>
                                                        </div>
                                                    </ItemTemplate>                                              
                                                 </asp:DataList>
                                              </ContentTemplate>
                                          </asp:UpdatePanel>
                                      </div>
                                   </div>
                        <article class="box page-content">
                        <asp:DataList ID="dlComentarios" runat="server" OnItemDataBound="dlComentarios_ItemDataBound">
                            <ItemTemplate>
                            <section>
                            <header>
                            <p><asp:Label runat="server" Text='<%#Eval("Nickname") %>'></asp:Label></p>
                            <ul class="meta">
							<li class="icon fa-clock-o"><asp:Label ID="lblTiempoPublicado" runat="server" Text='<%#Eval("Fecha")%>'></asp:Label></li>
						</ul>
                            <asp:Label ID="lblComentario" runat="server" Text='<%#Eval("Comentario") %>'></asp:Label>
                            </header>
                            </section>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:Panel ID="pnComentar" runat="server" Visible="false">
                                <header>
                                <h2>Dejanos tu comentario!</h2>
                                <asp:TextBox ID="txtCajaComentario" Text="" runat="server" TextMode="Multiline" style="resize:none; width:40%"></asp:TextBox>
                                <br />
                                <asp:Button ID="btnGuardarComentario" runat="server" Text="Comentar" OnClick="guardarComentario_Click" />
                                <br />
                                <br />
                                <p><asp:Label ID="lblError" runat="server" Visible="false"></asp:Label></p>
                                </header>
                        </asp:Panel>
                        </article>
                               </asp:Panel>
					       </div>
                       </div>
                   </div>            
               </div>
           </div>
<asp:HiddenField ID="hdScroll" runat="server" />
<asp:HiddenField ID="hdIDSerie" runat="server" />
<asp:HiddenField ID="hdTemporada" runat="server" />
<asp:HiddenField ID="hfElementPosition" runat="server" Value="2" />
</asp:Content>

