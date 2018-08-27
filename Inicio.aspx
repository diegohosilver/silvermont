<%@ Page Title="" Language="C#" MasterPageFile="~/SilverMont.master" AutoEventWireup="true" CodeFile="Inicio.aspx.cs" Inherits="Inicio" EnableEventValidation="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <script type="text/javascript">
        $(function ($) {
                var node = document.getElementById("nav").getElementsByTagName("li")[0];
                node.setAttribute("class", "current");
        });
        function alFondoNoticias() {
            $("html, body").animate({ scrollTop: 680 }, 700);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Main -->
    	<div id="main-wrapper">
		    <div id="main" class="container">
                        <div class="row 200%">
							    <div class="12u">

                                   <section class="box highlight">
										<header>
											<h2>Silvermont, un portal hecho por expertos, para expertos</h2>
											<p>Para empezar, te dejamos nuestra recomendacion personal</p>
										</header>
									</section>
								    <!-- Features -->

									    <section class="box features">
										    <div>
											    <div class="row">	    
                                                        <asp:ListView ID="lvSeriesPrincipales" runat="server" GroupItemCount="4" OnItemCommand="lvSeriesPrincipales_ItemCommand">
                                                            <ItemTemplate>
                                                                <td>
                                                                     <div id="asd" style="margin-left:25%">
                                                                         <h3>
                                                                                <a href="#" class="text">
                                                                                       <asp:Label ID="lblNombreSerie" runat="server" Text='<%# Eval("Nombre") %>'></asp:Label>
															                    </a>
															                </h3>
                                                                        <asp:ImageButton CssClass="image featured" ID="Miniatura" Height="152px" widht="400px" ImageUrl='<%# Eval("Miniatura")%>' CommandName="redirectSerie" CommandArgument='<%# Eval("id") %>' runat="server" />
															             
                                                              </div>
                                                                    </td>
                                                            </ItemTemplate>
                                                            <LayoutTemplate>
                                                                <table>
                                                                    <tr runat="server">
                                                                        <td id="groupPlaceholder" runat="server">

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <GroupTemplate>
                                                                <td id="itemPlaceholder" runat="server">

                                                                </td>
                                                            </GroupTemplate>
                                                        </asp:ListView>													    
												    </div>
                                                </div> 
									    </section>
							    </div>
                            </div>
                        <div class="row 200%">
							<div class="12u">

								<!-- Blog -->
									<section class="box blog">
										<h2 class="major"><span onclick="alFondoNoticias();" style="cursor:pointer; cursor:hand;">Ultimas noticias</span></h2>
											<div class="row">
												<div class="9u 12u(mobile)">
													<div class="content content-left">

														<!-- Featured Post -->
															<article class="box post">
																<header>
																	<h3><a href="#"></a><asp:Label ID="lblTitulo" runat="server"></asp:Label></h3>
																	<p><asp:Label ID="lblCopete" runat="server"></asp:Label></p>
																	<ul class="meta">
																		<li class="icon fa-clock-o"><asp:Label ID="lblTiempoPublicado" runat="server"></asp:Label></li>
																		<li class="icon fa-comments"><asp:Label ID="lblCantidadComentarios" runat="server"></asp:Label></li>
																	</ul>
																</header>
                                                                <asp:HiddenField ID="hdID" runat="server" />
																<asp:Image ID="imgNoticia" runat="server" CssClass="image featured"/>
																<p>
                                                                    <asp:Label ID="lblContenido" runat="server"></asp:Label>
																</p>
																<asp:Button ID="btnVerMas" runat="server" Text="Leer mas" CssClass="button" OnClick="btnVerMas_Click" />
															</article>

													</div>
												</div>
												<div class="3u 12u(mobile)">
													<div class="sidebar">

       													<!-- Archives -->
															<ul class="divided">
																<asp:DataList ID="dlArchivos" runat="server" OnItemCommand="dlArchivos_ItemCommand" OnItemDataBound="dlArchivos_ItemDataBound">
                                                                    <ItemTemplate>
                                                                        <li>
																	        <article class="box post-summary">
                                                                                <asp:HiddenField ID="hfIDAux" runat="server" Value='<%#Eval("id") %>' />
																		        <h3><asp:LinkButton ID="lblTituloArchivo" runat="server" Text='<%# Eval("Titulo") %>' CommandName="CargarNoticia" CommandArgument='<%# Eval("id") %>'></asp:LinkButton></a></h3>
																		        <ul class="meta">
																			        <li class="icon fa-clock-o"><asp:Label ID="lblTiempoPublicacion" runat="server" Text='<%# Eval("FechaPublicacion") %>'></asp:Label></li>
																			        <li class="icon fa-comments"><asp:Label ID="lblCantidadComentarios" runat="server"></asp:Label></li>
																		        </ul>
																	        </article>
																        </li>
                                                                        <br />
                                                                    </ItemTemplate>
                                                                </asp:DataList>
															</ul>                                                           
															<a href="NoticiasArchivo.aspx" class="button">Todas las noticias</a>
													</div>
												</div>
											</div>
									</section>
							    </div>
						    </div>
                         </div>
                     </div>
</asp:Content>

