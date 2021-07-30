using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Editor
{
    /// <summary>
    /// Interaction logic for MathToolbar.xaml
    /// </summary>
    public partial class EquationToolBar : UserControl
    {
        public event EventHandler CommandCompleted = (x, y) => { };
        Dictionary<object, ButtonPanel> buttonPanelMapping = new Dictionary<object, ButtonPanel>();
        ButtonPanel visiblePanel = null;

        public EquationToolBar()
        {
            InitializeComponent();
        }

        private void toolBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (visiblePanel != null)
            {
                visiblePanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (buttonPanelMapping[sender].Visibility != System.Windows.Visibility.Visible)
            {
                buttonPanelMapping[sender].Visibility = System.Windows.Visibility.Visible;
                visiblePanel = buttonPanelMapping[sender];
            }
        }

        public void HideVisiblePanel()
        {
            if (visiblePanel != null)
            {
                visiblePanel.Visibility = System.Windows.Visibility.Collapsed;
                visiblePanel = null;
            }
        }

        private void toolBarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ChangeActivePanel(sender);
        }
        
        private void toolBarButton_GotFocus(object sender, RoutedEventArgs e)
        {
            ChangeActivePanel(sender);
        }

        void ChangeActivePanel(object sender)
        {
            if (visiblePanel != null)
            {
                visiblePanel.Visibility = System.Windows.Visibility.Collapsed;
                buttonPanelMapping[sender].Visibility = System.Windows.Visibility.Visible;
                visiblePanel = buttonPanelMapping[sender];
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateBracketsPanel();
            CreateSumsProductsPanel();
            CreateIntegralsPanel();
            CreateSubAndSuperPanel();
            CreateDivAndRootsPanel();
            CreateCompositePanel();
            CreateDecoratedEquationPanel();
            CreateDecoratedCharacterPanel();
            CreateArrowEquationPanel();
            CreateBoxEquationPanel();
            CreateMatrixPanel();
        }

        void CreatePanel(List<CommandDetails> list, Button toolBarButton, int columns, int margin)
        {
            ButtonPanel bp = new ButtonPanel(list, columns, margin);
            bp.ButtonClick += (x, y) => { CommandCompleted(this, EventArgs.Empty); visiblePanel = null; };
            mainToolBarPanel.Children.Add(bp);
            Canvas.SetTop(bp, mainToolBarPanel.Height);
            Vector offset = VisualTreeHelper.GetOffset(toolBarButton);
            Canvas.SetLeft(bp, offset.X + 2);
            bp.Visibility = System.Windows.Visibility.Collapsed;
            buttonPanelMapping.Add(toolBarButton, bp);
        }

        void CreateImagePanel(Uri[] imageUris, CommandType[] commands, object[] paramz, Button toolBarButton, int columns)
        {
            Image[] items = new Image[imageUris.Count()];
            for (int i = 0; i < items.Count(); i++)
            {
                items[i] = new Image();
                BitmapImage bmi = new BitmapImage(imageUris[i]);
                items[i].Source = bmi;
            }
            List<CommandDetails> list = new List<CommandDetails>();
            for (int i = 0; i < items.Count(); i++)
            {
                list.Add(new CommandDetails { Image = items[i], CommandType = commands[i], CommandParam = paramz[i] });
            }
            CreatePanel(list, toolBarButton, columns, 4);
        }

        Uri CreateImageUri(string subFolder, string imageFileName)
        {
            return new Uri("pack://application:,,,/images/commands/" + subFolder + "/" + imageFileName);
        }

        void CreateBracketsPanel()
        {
            Uri[] imageUris = { CreateImageUri("brackets", "SingleBar.png"),
                                CreateImageUri("brackets", "DoubleBar.png"),
                                CreateImageUri("brackets", "Floor.png"),
                                CreateImageUri("brackets", "Ceiling.png"),
                                CreateImageUri("brackets", "CurlyBracket.png"),
                                CreateImageUri("brackets", "RightRightSquareBracket.png"),
                                CreateImageUri("brackets", "Parentheses.png"),
                                CreateImageUri("brackets", "SquareBracket.png"),
                                CreateImageUri("brackets", "AngleBar.png"),
                                CreateImageUri("brackets", "BarAngle.png"),
                                CreateImageUri("brackets", "SquareBar.png"),
                                CreateImageUri("brackets", "ParenthesisSquare.png"),
                                CreateImageUri("brackets", "SquareParenthesis.png"),
                                CreateImageUri("brackets", "LeftLeftSquareBracket.png"),
                                CreateImageUri("brackets", "PointingAngles.png"),
                                CreateImageUri("brackets", "RightLeftSquareBracket.png"),
                                CreateImageUri("brackets", "LeftCurlyBracket.png"),
                                CreateImageUri("brackets", "RightCurlyBracket.png"),
                                CreateImageUri("brackets", "LeftDoubleBar.png"),
                                CreateImageUri("brackets", "RightDoubleBar.png"),
                                CreateImageUri("brackets", "LeftParenthesis.png"),
                                CreateImageUri("brackets", "RightParenthesis.png"),
                                CreateImageUri("brackets", "LeftSquareBar.png"),
                                CreateImageUri("brackets", "RightSquareBar.png"),
                                CreateImageUri("brackets", "LeftSquareBracket.png"),
                                CreateImageUri("brackets", "RightSquareBracket.png"),
                                CreateImageUri("brackets", "LeftAngle.png"),
                                CreateImageUri("brackets", "RightAngle.png"),
                                CreateImageUri("brackets", "LeftBar.png"),
                                CreateImageUri("brackets", "RightBar.png"),
                                CreateImageUri("brackets", "TopCurlyBracket.png"),
                                CreateImageUri("brackets", "BottomCurlyBracket.png"),
                                CreateImageUri("brackets", "TopSquareBracket.png"),
                                CreateImageUri("brackets", "BottomSquareBracket.png"),
                                CreateImageUri("brackets", "DoubleArrowBarBracket.png"),                                
                               };

            CommandType[] commands = { CommandType.LeftRightBracket, CommandType.LeftRightBracket, CommandType.LeftRightBracket, 
                                       CommandType.LeftRightBracket, CommandType.LeftRightBracket, CommandType.LeftRightBracket,
                                       CommandType.LeftRightBracket, CommandType.LeftRightBracket, CommandType.LeftRightBracket, 
                                       CommandType.LeftRightBracket, CommandType.LeftRightBracket, CommandType.LeftRightBracket,
                                       CommandType.LeftRightBracket, CommandType.LeftRightBracket, CommandType.LeftRightBracket,
                                       CommandType.LeftRightBracket,
                                       CommandType.LeftBracket,      CommandType.RightBracket, 
                                       CommandType.LeftBracket,      CommandType.RightBracket, 
                                       CommandType.LeftBracket,      CommandType.RightBracket, 
                                       CommandType.LeftBracket,      CommandType.RightBracket, 
                                       CommandType.LeftBracket,      CommandType.RightBracket, 
                                       CommandType.LeftBracket,      CommandType.RightBracket, 
                                       CommandType.LeftBracket,      CommandType.RightBracket, 
                                       CommandType.TopBracket,  CommandType.BottomBracket,
                                       CommandType.TopBracket, CommandType.BottomBracket, 
                                       CommandType.DoubleArrowBarBracket,                                       
                                     };
            object[] paramz = { 
                                   new BracketSignType [] {BracketSignType.LeftBar,       BracketSignType.RightBar},
                                   new BracketSignType [] {BracketSignType.LeftDoubleBar, BracketSignType.RightDoubleBar},
                                   new BracketSignType [] {BracketSignType.LeftFloor,     BracketSignType.RightFloor},
                                   new BracketSignType [] {BracketSignType.LeftCeiling,   BracketSignType.RightCeiling},
                                   new BracketSignType [] {BracketSignType.LeftCurly,     BracketSignType.RightCurly},
                                   new BracketSignType [] {BracketSignType.RightSquare,   BracketSignType.RightSquare},
                                   new BracketSignType [] {BracketSignType.LeftRound,     BracketSignType.RightRound},
                                   new BracketSignType [] {BracketSignType.LeftSquare,    BracketSignType.RightSquare},
                                   new BracketSignType [] {BracketSignType.LeftAngle,     BracketSignType.RightBar},
                                   new BracketSignType [] {BracketSignType.LeftBar,       BracketSignType.RightAngle},
                                   new BracketSignType [] {BracketSignType.LeftSquareBar, BracketSignType.RightSquareBar},
                                   new BracketSignType [] {BracketSignType.LeftRound,     BracketSignType.RightSquare},
                                   new BracketSignType [] {BracketSignType.LeftSquare,    BracketSignType.RightRound},
                                   new BracketSignType [] {BracketSignType.LeftSquare,    BracketSignType.LeftSquare},
                                   new BracketSignType [] {BracketSignType.LeftAngle,     BracketSignType.RightAngle},                                   
                                   new BracketSignType [] {BracketSignType.RightSquare,   BracketSignType.LeftSquare},

                                   BracketSignType.LeftCurly,
                                   BracketSignType.RightCurly, 
                                   BracketSignType.LeftDoubleBar,
                                   BracketSignType.RightDoubleBar, 
                                   BracketSignType.LeftRound,
                                   BracketSignType.RightRound, 
                                   BracketSignType.LeftSquareBar,
                                   BracketSignType.RightSquareBar, 
                                   BracketSignType.LeftSquare,
                                   BracketSignType.RightSquare, 
                                   BracketSignType.LeftAngle,
                                   BracketSignType.RightAngle,
                                   BracketSignType.LeftBar,
                                   BracketSignType.RightBar, 
                                   HorizontalBracketSignType.TopCurly,
                                   HorizontalBracketSignType.BottomCurly,
                                   HorizontalBracketSignType.ToSquare,
                                   HorizontalBracketSignType.BottomSquare,
                                   0,
                              };

            CreateImagePanel(imageUris, commands, paramz, bracketsButton, 4);
        }

        void CreateSumsProductsPanel()
        {
            Uri[] imageUris = {   
                                  CreateImageUri("sumsProducts", "sum.png"),
                                  CreateImageUri("sumsProducts", "sumSub.png"),
                                  CreateImageUri("sumsProducts", "sumSubSuper.png"),
                                  CreateImageUri("sumsProducts", "sumBottom.png"),
                                  CreateImageUri("sumsProducts", "sumBottomTop.png"),                                  

                                  CreateImageUri("sumsProducts", "product.png"),
                                  CreateImageUri("sumsProducts", "productSub.png"),
                                  CreateImageUri("sumsProducts", "productSubSuper.png"),
                                  CreateImageUri("sumsProducts", "productBottom.png"),
                                  CreateImageUri("sumsProducts", "productBottomTop.png"),

                                  CreateImageUri("sumsProducts", "coProduct.png"),
                                  CreateImageUri("sumsProducts", "coProductSub.png"),
                                  CreateImageUri("sumsProducts", "coProductSubSuper.png"),
                                  CreateImageUri("sumsProducts", "coProductBottom.png"),
                                  CreateImageUri("sumsProducts", "coProductBottomTop.png"),
                                  
                                  CreateImageUri("sumsProducts", "intersection.png"),
                                  CreateImageUri("sumsProducts", "intersectionSub.png"),
                                  CreateImageUri("sumsProducts", "intersectionSubSuper.png"),
                                  CreateImageUri("sumsProducts", "intersectionBottom.png"),
                                  CreateImageUri("sumsProducts", "intersectionBottomTop.png"),
                                  
                                  CreateImageUri("sumsProducts", "union.png"),
                                  CreateImageUri("sumsProducts", "unionSub.png"),
                                  CreateImageUri("sumsProducts", "unionSubSuper.png"),
                                  CreateImageUri("sumsProducts", "unionBottom.png"),
                                  CreateImageUri("sumsProducts", "unionBottomTop.png"),
                              };
            CommandType[] commands = Enumerable.Repeat(CommandType.SignComposite, imageUris.Length).ToArray();
            object[] paramz = { 
                                  new object [] {Position.None,    SignCompositeSymbol.Sum} ,
                                  new object [] {Position.Sub,       SignCompositeSymbol.Sum} ,
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.Sum} ,
                                  new object [] {Position.Bottom,    SignCompositeSymbol.Sum} ,
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.Sum} ,

                                  new object [] {Position.None,    SignCompositeSymbol.Product} ,
                                  new object [] {Position.Sub,       SignCompositeSymbol.Product} ,
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.Product} ,
                                  new object [] {Position.Bottom,    SignCompositeSymbol.Product} ,
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.Product} ,

                                  new object [] {Position.None,    SignCompositeSymbol.CoProduct} ,
                                  new object [] {Position.Sub,       SignCompositeSymbol.CoProduct} ,
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.CoProduct} ,
                                  new object [] {Position.Bottom,    SignCompositeSymbol.CoProduct} ,
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.CoProduct} ,

                                  new object [] {Position.None,    SignCompositeSymbol.Intersection} ,
                                  new object [] {Position.Sub,       SignCompositeSymbol.Intersection} ,
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.Intersection} ,
                                  new object [] {Position.Bottom,    SignCompositeSymbol.Intersection} ,
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.Intersection} ,

                                  new object [] {Position.None,    SignCompositeSymbol.Union} ,
                                  new object [] {Position.Sub,       SignCompositeSymbol.Union} ,
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.Union} ,
                                  new object [] {Position.Bottom,    SignCompositeSymbol.Union} ,
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.Union} ,
                              };

            CreateImagePanel(imageUris, commands, paramz, sumsProductsButton, 5);
        }

        void CreateIntegralsPanel()
        {
            Uri[] imageUris = { 
                                  CreateImageUri("integrals/Single", "Simple.png"),
                                  CreateImageUri("integrals/Single", "Sub.png"),
                                  CreateImageUri("integrals/Single", "SubSuper.png"),
                                  CreateImageUri("integrals/Single", "Bottom.png"),
                                  CreateImageUri("integrals/Single", "BottomTop.png"),                                  

                                  CreateImageUri("integrals/Double", "Simple.png"),
                                  CreateImageUri("integrals/Double", "Sub.png"),
                                  CreateImageUri("integrals/Double", "SubSuper.png"),
                                  CreateImageUri("integrals/Double", "Bottom.png"),
                                  CreateImageUri("integrals/Double", "BottomTop.png"),

                                  CreateImageUri("integrals/Triple", "Simple.png"),
                                  CreateImageUri("integrals/Triple", "Sub.png"),
                                  CreateImageUri("integrals/Triple", "SubSuper.png"),
                                  CreateImageUri("integrals/Triple", "Bottom.png"),
                                  CreateImageUri("integrals/Triple", "BottomTop.png"),
                                  
                                  CreateImageUri("integrals/Contour", "Simple.png"),
                                  CreateImageUri("integrals/Contour", "Sub.png"),
                                  CreateImageUri("integrals/Contour", "SubSuper.png"),
                                  CreateImageUri("integrals/Contour", "Bottom.png"),
                                  CreateImageUri("integrals/Contour", "BottomTop.png"),

                                  CreateImageUri("integrals/Surface", "Simple.png"),
                                  CreateImageUri("integrals/Surface", "Sub.png"),
                                  CreateImageUri("integrals/Surface", "SubSuper.png"),
                                  CreateImageUri("integrals/Surface", "Bottom.png"),
                                  CreateImageUri("integrals/Surface", "BottomTop.png"),

                                  CreateImageUri("integrals/Volume", "Simple.png"),
                                  CreateImageUri("integrals/Volume", "Sub.png"),
                                  CreateImageUri("integrals/Volume", "SubSuper.png"),
                                  CreateImageUri("integrals/Volume", "Bottom.png"),
                                  CreateImageUri("integrals/Volume", "BottomTop.png"),

                                  CreateImageUri("integrals/Clock", "Simple.png"),
                                  CreateImageUri("integrals/Clock", "Sub.png"),
                                  CreateImageUri("integrals/Clock", "SubSuper.png"),
                                  CreateImageUri("integrals/Clock", "Bottom.png"),
                                  CreateImageUri("integrals/Clock", "BottomTop.png"),

                                  CreateImageUri("integrals/AntiClock", "Simple.png"),
                                  CreateImageUri("integrals/AntiClock", "Sub.png"),
                                  CreateImageUri("integrals/AntiClock", "SubSuper.png"),
                                  CreateImageUri("integrals/AntiClock", "Bottom.png"),
                                  CreateImageUri("integrals/AntiClock", "BottomTop.png"),
                               };

            CommandType[] commands = Enumerable.Repeat(CommandType.SignComposite, imageUris.Length).ToArray();

            object[] paramz = { 
                                  new object [] {Position.None,    SignCompositeSymbol.Integral},
                                  new object [] {Position.Sub,       SignCompositeSymbol.Integral},
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.Integral},
                                  new object [] {Position.Bottom,    SignCompositeSymbol.Integral},
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.Integral},
                                  

                                  new object [] {Position.None,    SignCompositeSymbol.DoubleIntegral},
                                  new object [] {Position.Sub,       SignCompositeSymbol.DoubleIntegral},
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.DoubleIntegral},
                                  new object [] {Position.Bottom,    SignCompositeSymbol.DoubleIntegral},
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.DoubleIntegral},
                                  
                                  
                                  new object [] {Position.None,    SignCompositeSymbol.TripleIntegral},
                                  new object [] {Position.Sub,       SignCompositeSymbol.TripleIntegral},
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.TripleIntegral},
                                  new object [] {Position.Bottom,    SignCompositeSymbol.TripleIntegral},
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.TripleIntegral},

                                  new object [] {Position.None,    SignCompositeSymbol.ContourIntegral},
                                  new object [] {Position.Sub,       SignCompositeSymbol.ContourIntegral},
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.ContourIntegral},
                                  new object [] {Position.Bottom,    SignCompositeSymbol.ContourIntegral},
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.ContourIntegral},

                                 new object [] {Position.None,    SignCompositeSymbol.SurfaceIntegral},
                                  new object [] {Position.Sub,       SignCompositeSymbol.SurfaceIntegral},
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.SurfaceIntegral},
                                  new object [] {Position.Bottom,    SignCompositeSymbol.SurfaceIntegral},
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.SurfaceIntegral},

                                  new object [] {Position.None,    SignCompositeSymbol.VolumeIntegral},
                                  new object [] {Position.Sub,       SignCompositeSymbol.VolumeIntegral},
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.VolumeIntegral},
                                  new object [] {Position.Bottom,    SignCompositeSymbol.VolumeIntegral},
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.VolumeIntegral},

                                  new object [] {Position.None,    SignCompositeSymbol.ClockContourIntegral},
                                  new object [] {Position.Sub,       SignCompositeSymbol.ClockContourIntegral},
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.ClockContourIntegral},
                                  new object [] {Position.Bottom,    SignCompositeSymbol.ClockContourIntegral},
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.ClockContourIntegral},

                                  new object [] {Position.None,    SignCompositeSymbol.AntiClockContourIntegral},
                                  new object [] {Position.Sub,       SignCompositeSymbol.AntiClockContourIntegral},
                                  new object [] {Position.SubAndSuper,  SignCompositeSymbol.AntiClockContourIntegral},
                                  new object [] {Position.Bottom,    SignCompositeSymbol.AntiClockContourIntegral},
                                  new object [] {Position.BottomAndTop, SignCompositeSymbol.AntiClockContourIntegral},                                 
                              };

            CreateImagePanel(imageUris, commands, paramz, integralsButton, 5);
        }

        void CreateSubAndSuperPanel()
        {
            Uri[] imageUris = { 
                                  CreateImageUri("subSuper", "Sub.png"),   
                                  CreateImageUri("subSuper", "Super.png"),   
                                  CreateImageUri("subSuper", "SubSuper.png"),   
                                  CreateImageUri("subSuper", "SubLeft.png"),   
                                  CreateImageUri("subSuper", "SuperLeft.png"),   
                                  CreateImageUri("subSuper", "SubSuperLeft.png"),   
                               };
            CommandType[] commands = { CommandType.Sub, CommandType.Super, CommandType.SubAndSuper,
                                       CommandType.Sub, CommandType.Super, CommandType.SubAndSuper};

            object[] paramz = { Position.Right, Position.Right, Position.Right,
                                Position.Left, Position.Left, Position.Left,};

            CreateImagePanel(imageUris, commands, paramz, subAndSuperButton, 3);
        }

        void CreateCompositePanel()
        {
            Uri[] imageUris = { 
                                  CreateImageUri("composite", "CompositeBottom.png"),  
                                  CreateImageUri("composite", "CompositeTop.png"),  
                                  CreateImageUri("composite", "CompositeBottomTop.png"),                            
                                  CreateImageUri("composite", "BigBottom.png"),  
                                  CreateImageUri("composite", "BigTop.png"),  
                                  CreateImageUri("composite", "BigBottomTop.png"),                            
                                  CreateImageUri("composite", "BigSub.png"),  
                                  CreateImageUri("composite", "BigSuper.png"),  
                                  CreateImageUri("composite", "BigSubSuper.png"),                        
                               };
            CommandType[] commands = { 
                                         CommandType.Composite, CommandType.Composite, CommandType.Composite,
                                         CommandType.CompositeBig,    CommandType.CompositeBig, CommandType.CompositeBig, 
                                         CommandType.CompositeBig, CommandType.CompositeBig, CommandType.CompositeBig,                                          
                                     };

            object[] paramz = { 
                                  Position.Bottom, Position.Top, Position.BottomAndTop,
                                  Position.Bottom, Position.Top, Position.BottomAndTop,
                                  Position.Sub, Position.Super, Position.SubAndSuper,
                              };

            CreateImagePanel(imageUris, commands, paramz, compositeButton, 3);
        }

        void CreateDecoratedEquationPanel()
        {
            Uri[] imageUris = { 
                                  CreateImageUri("decorated/equation", "hat.png"),  
                                  CreateImageUri("decorated/equation", "tilde.png"),  
                                  CreateImageUri("decorated/equation", "parenthesis.png"),                            
                                  CreateImageUri("decorated/equation", "tortoise.png"),  
                                  CreateImageUri("decorated/equation", "topBar.png"),  
                                  CreateImageUri("decorated/equation", "topDoubleBar.png"),                            
                                  CreateImageUri("decorated/equation", "topRightArrow.png"),  
                                  CreateImageUri("decorated/equation", "topLeftArrow.png"),  
                                  CreateImageUri("decorated/equation", "topRightHalfArrow.png"),                            
                                  CreateImageUri("decorated/equation", "topLeftHalfArrow.png"),  
                                  CreateImageUri("decorated/equation", "topDoubleArrow.png"),  
                                    
                                  CreateImageUri("decorated/equation", "topDoubleArrow.png"),  //to be left empty

                                  CreateImageUri("decorated/equation", "bottomBar.png"),                            
                                  CreateImageUri("decorated/equation", "bottomDoubleBar.png"),  
                                  CreateImageUri("decorated/equation", "bottomRightArrow.png"),  
                                  CreateImageUri("decorated/equation", "bottomLeftArrow.png"),  
                                  CreateImageUri("decorated/equation", "bottomRightHalfArrow.png"),  
                                  CreateImageUri("decorated/equation", "bottomLeftHalfArrow.png"),  
                                  CreateImageUri("decorated/equation", "bottomDoubleArrow.png"),                 
           
                                  CreateImageUri("decorated/equation", "bottomDoubleArrow.png"),  //to be left empty
                                  
                                  CreateImageUri("decorated/equation", "cross.png"),  
                                  CreateImageUri("decorated/equation", "leftCross.png"),  
                                  CreateImageUri("decorated/equation", "rightCross.png"),                            
                                  CreateImageUri("decorated/equation", "strikeThrough.png"),  
                               };
            CommandType[] commands = Enumerable.Repeat(CommandType.Decorated, imageUris.Length).ToArray();
            commands[11] = CommandType.None; //empty cell
            commands[19] = CommandType.None; //empty cell

            object[] paramz = {                                   
                                  new object [] {DecorationType.Hat,                    Position.Top },
                                  new object [] {DecorationType.Tilde,                  Position.Top },
                                  new object [] {DecorationType.Parenthesis,            Position.Top },
                                  new object [] {DecorationType.Tortoise,               Position.Top },
                                  new object [] {DecorationType.Bar,                    Position.Top },
                                  new object [] {DecorationType.DoubleBar,              Position.Top },
                                  new object [] {DecorationType.RightArrow,             Position.Top },
                                  new object [] {DecorationType.LeftArrow,              Position.Top },
                                  new object [] {DecorationType.RightHarpoonUpBarb,     Position.Top },
                                  new object [] {DecorationType.LeftHarpoonUpBarb,      Position.Top },
                                  new object [] {DecorationType.DoubleArrow,            Position.Top },
                                  0, //empty cell                                  
                                  new object [] {DecorationType.Bar,                    Position.Bottom },
                                  new object [] {DecorationType.DoubleBar,              Position.Bottom },
                                  new object [] {DecorationType.RightArrow,             Position.Bottom },
                                  new object [] {DecorationType.LeftArrow,              Position.Bottom },
                                  new object [] {DecorationType.RightHarpoonDownBarb,   Position.Bottom },
                                  new object [] {DecorationType.LeftHarpoonDownBarb,    Position.Bottom },
                                  new object [] {DecorationType.DoubleArrow,            Position.Bottom },
                                  0, //empty cell
                                  new object [] {DecorationType.Cross,          Position.Middle },
                                  new object [] {DecorationType.LeftCross,      Position.Middle },
                                  new object [] {DecorationType.RightCross,     Position.Middle },
                                  new object [] {DecorationType.StrikeThrough,  Position.Middle },
                              };
            CreateImagePanel(imageUris, commands, paramz, decoratedEquationButton, 4);
        }
        
        void CreateDecoratedCharacterPanel()
        {
            Uri[] imageUris = {   
                                  CreateImageUri("decorated/character", "None.png"),
                                  CreateImageUri("decorated/character", "StrikeThrough.png"),
                                  CreateImageUri("decorated/character", "DoubleStrikeThrough.png"),                                  
                                  CreateImageUri("decorated/character", "LeftCross.png"), 
                                  CreateImageUri("decorated/character", "RightCross.png"),         
                                  CreateImageUri("decorated/character", "Cross.png"),
                                  CreateImageUri("decorated/character", "VstrikeThrough.png"),
                                  CreateImageUri("decorated/character", "VDoubleStrikeThrough.png"), 
                                  CreateImageUri("decorated/character", "LeftUprightCross.png"), 
                                  CreateImageUri("decorated/character", "RightUprightCross.png"), 
                                  
                                  CreateImageUri("decorated/character", "Prime.png"), 
                                  CreateImageUri("decorated/character", "DoublePrime.png"),                                   
                                  CreateImageUri("decorated/character", "TriplePrime.png"),
                                  CreateImageUri("decorated/character", "ReversePrime.png"),
                                  CreateImageUri("decorated/character", "ReverseDoublePrime.png"),

                                  CreateImageUri("decorated/character", "AcuteAccent.png"), 
                                  CreateImageUri("decorated/character", "GraveAccent.png"),                                   
                                  CreateImageUri("decorated/character", "TopRing.png"),
                                  CreateImageUri("decorated/character", "TopRightRing.png"),
                                  CreateImageUri("decorated/character", "ReverseDoublePrime.png"), //Empty

                                  CreateImageUri("decorated/character", "TopBar.png"), 
                                  CreateImageUri("decorated/character", "TopTilde.png"),                                   
                                  CreateImageUri("decorated/character", "TopBreve.png"),
                                  CreateImageUri("decorated/character", "TopInvertedBreve.png"),
                                  CreateImageUri("decorated/character", "TopCircumflex.png"),

                                  CreateImageUri("decorated/character", "BottomBar.png"), 
                                  CreateImageUri("decorated/character", "BottomTilde.png"),                                   
                                  CreateImageUri("decorated/character", "BottomBreve.png"),
                                  CreateImageUri("decorated/character", "BottomInvertedBreve.png"),
                                  CreateImageUri("decorated/character", "TopCaron.png"),

                                  CreateImageUri("decorated/character", "TopRightArrow.png"), 
                                  CreateImageUri("decorated/character", "TopLeftArrow.png"),                                   
                                  CreateImageUri("decorated/character", "TopDoubleArrow.png"),
                                  CreateImageUri("decorated/character", "TopRightHarpoon.png"),
                                  CreateImageUri("decorated/character", "TopLeftHarpoon.png"),


                                  CreateImageUri("decorated/character", "BottomRightArrow.png"), 
                                  CreateImageUri("decorated/character", "BottomLeftArrow.png"),                                   
                                  CreateImageUri("decorated/character", "BottomDoubleArrow.png"),
                                  CreateImageUri("decorated/character", "BottomRightHarpoon.png"),
                                  CreateImageUri("decorated/character", "BottomLeftHarpoon.png"),

                                  CreateImageUri("decorated/character", "TopDot.png"), 
                                  CreateImageUri("decorated/character", "TopDDot.png"),                                   
                                  CreateImageUri("decorated/character", "TopTDot.png"),
                                  CreateImageUri("decorated/character", "TopFourDot.png"),
                                  CreateImageUri("decorated/character", "TopFourDot.png"), //Empty
                                  
                                  CreateImageUri("decorated/character", "BottomDot.png"), 
                                  CreateImageUri("decorated/character", "BottomDDot.png"),                                   
                                  CreateImageUri("decorated/character", "BottomTDot.png"),
                                  CreateImageUri("decorated/character", "BottomFourDot.png"),
                                  CreateImageUri("decorated/character", "BottomFourDot.png"), //Empty
                                                                  
                               };
            CommandType[] commands = Enumerable.Repeat(CommandType.DecoratedCharacter, imageUris.Length).ToArray();
            commands[19] = CommandType.None; //empty cell 
            commands[44] = CommandType.None; //empty cell           
            commands[49] = CommandType.None; //empty cell  

            object[] paramz = {                                   
                                  new object [] {CharacterDecorationType.None,                  Position.Over, null},
                                  new object [] {CharacterDecorationType.StrikeThrough,         Position.Over, null},
                                  new object [] {CharacterDecorationType.DoubleStrikeThrough,   Position.Over, null},
                                  new object [] {CharacterDecorationType.LeftCross,             Position.Over, null},
                                  new object [] {CharacterDecorationType.RightCross,            Position.Over, null},
                                  new object [] {CharacterDecorationType.Cross,                 Position.Over, null},
                                  new object [] {CharacterDecorationType.VStrikeThrough,        Position.Over, null},
                                  new object [] {CharacterDecorationType.VDoubleStrikeThrough,  Position.Over, null},  
                                  new object [] {CharacterDecorationType.LeftUprightCross,      Position.Over, null},
                                  new object [] {CharacterDecorationType.RightUprightCross,     Position.Over, null},  
                                  
                                  new object [] {CharacterDecorationType.Unicode, Position.TopRight,  "\u2032"}, //Prime
                                  new object [] {CharacterDecorationType.Unicode, Position.TopRight,  "\u2033"}, //Double prime
                                  new object [] {CharacterDecorationType.Unicode, Position.TopRight,  "\u2034"}, //Triple prime
                                  new object [] {CharacterDecorationType.Unicode, Position.TopLeft,   "\u2035"}, //Reversed prime
                                  new object [] {CharacterDecorationType.Unicode, Position.TopLeft,   "\u2036"}, //Double reversed prime

                                  new object [] {CharacterDecorationType.Unicode, Position.Top,  "\u02CA"}, // Acute
                                  new object [] {CharacterDecorationType.Unicode, Position.Top,  "\u02CB"}, //Grave
                                  new object [] {CharacterDecorationType.Unicode, Position.Top,  "\u030A"}, //Ring
                                  new object [] {CharacterDecorationType.Unicode, Position.TopRight,  "\u030A"}, //Ring
                                  0, //Empty
                                  
                                  new object [] {CharacterDecorationType.Unicode, Position.Top,  "\u0332"}, //Bar or line
                                  new object [] {CharacterDecorationType.Unicode, Position.Top,  "\u0334"}, //Tilde
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u0306"}, //Breve
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u0311"}, //Inverted Breve
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u02C6"}, //Circumflex

                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u0332"}, //Bar or line
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u0334"}, //Tilde
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u0306"}, //Breve
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u0311"}, //Inverted breve
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u02C7"}, //Caron or check

                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u20D7"}, //left arrow
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u20D6"}, //right arrow
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u20E1"}, //double arrow
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u20D1"}, //top right harpoon
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u20D0"}, //top left harpoon

                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u20D7"}, //left arrow
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u20D6"}, //right arrow
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u20E1"}, //double arrow
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u20EC"}, //bottom right harpoon
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u20ED"}, //bottom left harpoon

                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u0323"},  //dot
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u0324"},  //two dots
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u20DB" }, //three dots
                                  new object [] {CharacterDecorationType.Unicode, Position.Top, "\u20DC" }, //four dots
                                  0, //Empty
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u0323"},  //dot
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u0324"},  //two dots
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u20DB" }, //three dots
                                  new object [] {CharacterDecorationType.Unicode, Position.Bottom, "\u20DC" }, //four dots
                                  0, //Empty
                              };
            CreateImagePanel(imageUris, commands, paramz, decoratedCharacterButton, 5);
        }

        void CreateArrowEquationPanel()
        {
            Uri[] imageUris = {                                                                                                 
                                  CreateImageUri("decorated/arrow", "LeftTop.png"),                            
                                  CreateImageUri("decorated/arrow", "LeftBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "LeftBottomTop.png"),    

                                  CreateImageUri("decorated/arrow", "RightTop.png"),                            
                                  CreateImageUri("decorated/arrow", "RightBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "RightBottomTop.png"),    

                                  CreateImageUri("decorated/arrow", "DoubleTop.png"),                            
                                  CreateImageUri("decorated/arrow", "DoubleBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "DoubleBottomTop.png"),    

                                  CreateImageUri("decorated/arrow", "RightLeftTop.png"),                            
                                  CreateImageUri("decorated/arrow", "RightLeftBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "RightLeftBottomTop.png"),    

                                  CreateImageUri("decorated/arrow", "RightSmallLeftTop.png"),                            
                                  CreateImageUri("decorated/arrow", "RightSmallLeftBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "RightSmallLeftBottomTop.png"),    

                                  CreateImageUri("decorated/arrow", "SmallRightLeftTop.png"),                            
                                  CreateImageUri("decorated/arrow", "SmallRightLeftBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "SmallRightLeftBottomTop.png"),

                                  CreateImageUri("decorated/arrow", "RightLeftHarpTop.png"),                            
                                  CreateImageUri("decorated/arrow", "RightLeftHarpBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "RightLeftHarpBottomTop.png"),    

                                  CreateImageUri("decorated/arrow", "RightSmallLeftHarpTop.png"),                            
                                  CreateImageUri("decorated/arrow", "RightSmallLeftHarpBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "RightSmallLeftHarpBottomTop.png"),    

                                  CreateImageUri("decorated/arrow", "SmallRightLeftHarpTop.png"),                            
                                  CreateImageUri("decorated/arrow", "SmallRightLeftHarpBottom.png"),                            
                                  CreateImageUri("decorated/arrow", "SmallRightLeftHarpBottomTop.png"),
                               };
            CommandType[] commands = Enumerable.Repeat(CommandType.Arrow, imageUris.Length).ToArray();

            object[] paramz = {   
                                  new object [] {ArrowType.LeftArrow,               Position.Top },
                                  new object [] {ArrowType.LeftArrow,               Position.Bottom },
                                  new object [] {ArrowType.LeftArrow,               Position.BottomAndTop },

                                  new object [] {ArrowType.RightArrow,              Position.Top },
                                  new object [] {ArrowType.RightArrow,              Position.Bottom },
                                  new object [] {ArrowType.RightArrow,              Position.BottomAndTop },

                                  
                                  new object [] {ArrowType.DoubleArrow,             Position.Top },
                                  new object [] {ArrowType.DoubleArrow,             Position.Bottom },
                                  new object [] {ArrowType.DoubleArrow,             Position.BottomAndTop },

                                  
                                  new object [] {ArrowType.RightLeftArrow,          Position.Top },
                                  new object [] {ArrowType.RightLeftArrow,          Position.Bottom },
                                  new object [] {ArrowType.RightLeftArrow,          Position.BottomAndTop },

                                  
                                  new object [] {ArrowType.RightSmallLeftArrow,     Position.Top },
                                  new object [] {ArrowType.RightSmallLeftArrow,     Position.Bottom },
                                  new object [] {ArrowType.RightSmallLeftArrow,     Position.BottomAndTop },

                                  
                                  new object [] {ArrowType.SmallRightLeftArrow,     Position.Top },
                                  new object [] {ArrowType.SmallRightLeftArrow,     Position.Bottom },
                                  new object [] {ArrowType.SmallRightLeftArrow,     Position.BottomAndTop },

                                  
                                  new object [] {ArrowType.RightLeftHarpoon,        Position.Top },
                                  new object [] {ArrowType.RightLeftHarpoon,        Position.Bottom },
                                  new object [] {ArrowType.RightLeftHarpoon,        Position.BottomAndTop },

                                  
                                  new object [] {ArrowType.RightSmallLeftHarpoon,     Position.Top },
                                  new object [] {ArrowType.RightSmallLeftHarpoon,     Position.Bottom },
                                  new object [] {ArrowType.RightSmallLeftHarpoon,     Position.BottomAndTop },

                                  
                                  new object [] {ArrowType.SmallRightLeftHarpoon,    Position.Top },
                                  new object [] {ArrowType.SmallRightLeftHarpoon,    Position.Bottom },
                                  new object [] {ArrowType.SmallRightLeftHarpoon,    Position.BottomAndTop },

                              };
            CreateImagePanel(imageUris, commands, paramz, arrowEquationButton, 3);
        }
        
        void CreateDivAndRootsPanel()
        {
            Uri[] imageUris = { 
                                  CreateImageUri("divAndRoots", "SqRoot.png"),  
                                  CreateImageUri("divAndRoots", "nRoot.png"),  
                                  CreateImageUri("divAndRoots", "DivMath.png"),  
                                  CreateImageUri("divAndRoots", "DivMathWithTop.png"),  

                                  CreateImageUri("divAndRoots", "Div.png"),  
                                  CreateImageUri("divAndRoots", "DivDoubleBar.png"),  
                                  CreateImageUri("divAndRoots", "DivTripleBar.png"),
                                  CreateImageUri("divAndRoots", "SmallDiv.png"),

                                  CreateImageUri("divAndRoots", "DivSlant.png"),  
                                  CreateImageUri("divAndRoots", "SmallDivSlant.png"),
                                  CreateImageUri("divAndRoots", "DivHoriz.png"),
                                  CreateImageUri("divAndRoots", "SmallDivHoriz.png"),

                                  CreateImageUri("divAndRoots", "DivMathInverted.png"),  
                                  CreateImageUri("divAndRoots", "DivMathInvertedWithBottom.png"),
                                  CreateImageUri("divAndRoots", "DivTriangleFixed.png"),
                                  CreateImageUri("divAndRoots", "DivTriangleExpanding.png"),
                               };
            CommandType[] commands = { 
                                         CommandType.SquareRoot, CommandType.nRoot, 
                                         CommandType.Division, CommandType.Division, CommandType.Division,
                                         CommandType.Division, CommandType.Division, CommandType.Division,
                                         CommandType.Division, CommandType.Division, CommandType.Division,
                                         CommandType.Division, CommandType.Division, CommandType.Division,
                                          CommandType.Division, CommandType.Division,
                                     };
            object[] paramz = { 
                                  0, 0, //square root and nRoot
                                  DivisionType.DivMath, DivisionType.DivMathWithTop,
                                  DivisionType.DivRegular, DivisionType.DivDoubleBar, DivisionType.DivTripleBar,
                                  DivisionType.DivRegularSmall, DivisionType.DivSlanted, DivisionType.DivSlantedSmall, 
                                  DivisionType.DivHoriz, DivisionType.DivHorizSmall, DivisionType.DivMathInverted,
                                  DivisionType.DivInvertedWithBottom, DivisionType.DivTriangleFixed,
                                  DivisionType.DivTriangleExpanding,
                              };
            CreateImagePanel(imageUris, commands, paramz, divAndRootsButton, 4);
        }

        void CreateBoxEquationPanel()
        {
            Uri[] imageUris = { 
                                  CreateImageUri("box", "leftTop.png"),  
                                  CreateImageUri("box", "leftBottom.png"),  
                                  CreateImageUri("box", "rightTop.png"),  
                                  CreateImageUri("box", "rightBottom.png"),  
                                  CreateImageUri("box", "all.png"),
                               };
            CommandType[] commands = Enumerable.Repeat<CommandType>(CommandType.Box, imageUris.Length).ToArray();
            object[] paramz = { BoxType.LeftTop, BoxType.LeftBottom, BoxType.RightTop, BoxType.RightBottom, BoxType.All };
            CreateImagePanel(imageUris, commands, paramz, boxButton, 2);
        }

        void CreateMatrixPanel()
        {
            Uri[] imageUris = { 
                                  CreateImageUri("matrix", "2cellRow.png"),  
                                  CreateImageUri("matrix", "2cellColumn.png"),  
                                  CreateImageUri("matrix", "2Square.png"),

                                  CreateImageUri("matrix", "3cellRow.png"),  
                                  CreateImageUri("matrix", "3cellColumn.png"),  
                                  CreateImageUri("matrix", "3Square.png"),
                                  
                                  CreateImageUri("matrix", "row.png"),
                                  CreateImageUri("matrix", "column.png"),
                                  CreateImageUri("matrix", "custom.png"),
                               };
            CommandType[] commands = Enumerable.Repeat<CommandType>(CommandType.Matrix, imageUris.Length).ToArray();
            commands[6] = CommandType.CustomMatrix;
            commands[7] = CommandType.CustomMatrix;
            commands[8] = CommandType.CustomMatrix;
            object[] paramz = {
                                  new [] {1, 2},
                                  new [] {2, 1},
                                  new [] {2, 2},
                                  new [] {1, 3},
                                  new [] {3, 1},
                                  new [] {3, 3},
                                  new [] {1, 4},
                                  new [] {4, 1},
                                  new [] {4, 4},
                              };
            CreateImagePanel(imageUris, commands, paramz, matrixButton, 3);
        }
    }
}
