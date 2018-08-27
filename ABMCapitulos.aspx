<%@ Page Title="" Language="C#" MasterPageFile="~/SilverMont.master" AutoEventWireup="true" CodeFile="ABMCapitulos.aspx.cs" Inherits="ABMCapitulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
            <script type="text/javascript">
                $(function ($) {
                    var node = document.getElementById("nav").getElementsByTagName("li")[4];
                    node.setAttribute("class", "current");
                    BindEvents();
                });
                function abrirCerrar() {
                    $('.collapsible-panels div').slideToggle(350);
                }
                function ConfirmDelete() {
                    var delete_value = document.createElement("INPUT");
                    delete_value.type = "hidden";
                    delete_value.name = "delete_value";
                    if (confirm("Esta seguro de eliminar el capitulo?")) {
                        delete_value.value = "Yes";
                    } else {
                        delete_value.value = "No";
                    }
                    document.forms[0].appendChild(delete_value);
                }
                function BindEvents() {
                    if (document.getElementById('<%= hfPanelMostrar.ClientID%>').value == '0') {
                        $('.collapsible-panels div').hide();
                    }
                    else if (document.getElementById('<%= hfPanelMostrar.ClientID%>').value == '1') {
                        $('.collapsible-panels div').show();
                    }
                }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="main-wrapper">
		    <div id="main" class="container">
                    <div class="row 200%">
					        <div class="12u">
					            <div>
                                    <br />
                                      <h2 class="major"><span title="Click para desplegar" style="cursor:pointer; cursor:hand;" onclick="abrirCerrar();">Datos del Capitulo</span></h2>
                                        <div class="12u">
								            <div class="content">
                                                <asp:UpdatePanel ID="pnContenido" runat="server">
                                                         <ContentTemplate>
                                                <div class="collapsible-panels">                                            
									            <!-- Content -->
                                                    <div>
                                                        
										            <article class="box page-content">
                                                        <section class="box highlight">
                                                         <header>
                                                             <p style="display:inline-block">Serie<asp:DropDownList ID="ddlSeries" runat="server" OnSelectedIndexChanged="ddlSeries_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>Temporadas<asp:DropDownList ID="ddlTemporadas" runat="server" OnSelectedIndexChanged="ddlTemporadas_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList> Numero<asp:TextBox ID="txtNumero" runat="server" style="margin-left:10px; width:200px;"></asp:TextBox></p>
                                                             <br />
                                                             <br />
                                                             <p style="display:inline-block">Nombre del capitulo<asp:TextBox ID="txtNombreCapitulo" runat="server" style="margin-left:10px; width:400px"></asp:TextBox>
                                                             </p>
                                                             <br />
                                                             <br />                                                                                                               
                                                             <asp:Button ID="btnGuardar" runat="server" Text="Guardar Capitulo" OnClick="guardarCapitulo_Click" />
                                                             <asp:Button ID="btnModificar" runat="server" Text="Guardar Capitulo" OnClick="modificarCapitulo_Click" Visible="False" />
                                                             <br />
                                                             <br /> 
                                                             <p><asp:Label ID="lblError" runat="server" Visible="false" style="color:red"></asp:Label></p>                                                              
                                                         </header>
                                                       </section>                                                     
										            </article>
                                                      
                                                    </div>                                                                                                 
                                                </div> 
                                                </ContentTemplate>
                                                        <Triggers>
                                                            <asp:PostBackTrigger ControlID="ddlSeries" />
                                                            <asp:AsyncPostBackTrigger ControlID="ddlTemporadas" EventName="SelectedIndexChanged" />
                                                            <asp:PostBackTrigger ControlID="btnGuardar" />
                                                            <asp:PostBackTrigger ControlID="btnModificar" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
								            </div>
							            </div>
                                      <h2 class="major"><span style="cursor:default;">Listado de capitulos</span></h2>
                                    <center>
                                        <asp:GridView ID="gvNoticias" runat="server" HorizontalAlign="Center" AllowPaging="True" PageSize="10" OnPageIndexChanging="gvNoticias_PageIndexChanging"
                                            OnRowCommand="gvNoticias_OnRowCommand"
                                            CssClass="Grid"  
                                            Width="40%" BorderColor="Black"            
                                              AlternatingRowStyle-CssClass="alt"
                                              PagerStyle-CssClass="pgr">
                                            <RowStyle HorizontalAlign="Center" Wrap="False"  Font-Size="15px"/>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnModificar" ToolTip="Editar Serie" ImageUrl="/images/btnEditarSmall.png" runat="server" CommandName="MODIFICAR" CommandArgument='<%# Eval("id")%>'/>
                                                        <asp:ImageButton ID="btnEliminar" ToolTip="Eliminar Serie" ImageUrl="/images/btnBorrarSmall.png" runat="server" CommandName="ELIMINAR" OnClientClick="ConfirmDelete();" CommandArgument='<%# Eval("id")%>'/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <SelectedRowStyle Wrap="True" />
                                            <HeaderStyle CssClass="GrillasHeaders" ForeColor="Black" />
                                            <AlternatingRowStyle CssClass="GrillasFilaAlternate" />
                                        </asp:GridView>
                                    </center>
					                </div>
						        </div>
				            </div>
                        </div>
                    </div>
    <asp:HiddenField ID="hfPanelMostrar" runat="server" />
    <asp:HiddenField ID="hfIDCapitulo" runat="server" />
</asp:Content>

