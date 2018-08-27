<%@ Page Title="" Language="C#" MasterPageFile="~/SilverMont.master" AutoEventWireup="true" CodeFile="Noticias.aspx.cs" Inherits="Noticias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
            <script type="text/javascript">
                $(function ($) {
                    var node = document.getElementById("nav").getElementsByTagName("li")[1];
                    node.setAttribute("class", "current");
                });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="main-wrapper">
		    <div id="main" class="container">
                    <div class="row 200%">
					        <div class="12u">
					                <div>
                                            <br />
                                            <h2 class="major"><span>Noticias</span></h2>
                                            <div class="12u">
								            <div class="content">

									            <!-- Content -->

										            <article class="box page-content">

											            <header>
												            <h2><asp:Label ID="lblTituloNoticia" runat="server"></asp:Label></h2>
												            <p><asp:Label ID="lblCopete" runat="server"></asp:Label></p>
												            <ul class="meta">
													            <li class="icon fa-clock-o"><asp:Label ID="lblTiempoPublicacion" runat="server"></asp:Label></li>
													            <li class="icon fa-comments"><asp:Label ID="lblCantidadComentarios" runat="server"></asp:Label></li>
												            </ul>
											            </header>

											            <section>
                                                            <div>
                                                             <asp:Image ID="imgNoticia" runat="server" CssClass="image featured" />
                                                            </div>
											            </section>

											            <section>
												            <p>
													            <asp:Label ID="lblContenido" runat="server"></asp:Label>
												            </p>
											            </section>

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
                                                                <asp:Textbox ID="txtContenidoComentario" runat="server" TextMode="Multiline" style="resize:none; width:40%"></asp:Textbox>
                                                                <br />
                                                                <asp:Button ID="btnGuardarComentario" runat="server" Text="Comentar" OnClick="guardarComentario_Click" />
                                                                <br />
                                                                <br />
                                                                <p><asp:Label ID="lblError" runat="server" Visible="false"></asp:Label></p>
                                                                </header>
                                                        </asp:Panel>                                                       
										            </article>
								                  </div>
							               </div>
					                </div>
						    </div>
				    </div>
            </div>
        </div>
    <asp:HiddenField ID="hfIDNoticia" runat="server" />
</asp:Content>

