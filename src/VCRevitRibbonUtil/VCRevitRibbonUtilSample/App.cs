/*
 * Copyright 2012 � Victor Chekalin
 *
 * THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
 * KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
 * PARTICULAR PURPOSE.
 *
 */

#region Namespaces

using Autodesk.Revit.UI;
using VCRevitRibbonUtil;
using VCRevitRibbonUtilSample.Properties;

#endregion Namespaces

namespace VCRevitRibbonUtilSample
{
    internal class App : IExternalApplication
    {
        private static readonly string _namespace_prefix = typeof(App).Namespace + ".";

        public Result OnStartup(UIControlledApplication a)
        {
            Ribbon ribbon = new Ribbon(a);

            ribbon.Tab("MyTab")
                .Panel("Panel1")

                .CreateButton("btn1",
                    "Button1",
                    typeof(Command1),
                    btn => btn
                        .SetImage(Resources
                            ._1348119708_face_monkey_32)
                        .SetContextualHelp(ContextualHelpType.Url, "http://adn-cis.org")
                        .SetAvailability(_namespace_prefix + "ExtendAvailabilityZeroDocuments"))

                .CreateSeparator()

                .CreateButton<Command2>("btn2",
                    "Button2",
                    btn => btn
                        .SetLongDescription("This is a description of the button2")
                        .SetLargeImage(Resources
                            ._1348119643_face_glasses_32))

                .CreateStackedItems(si =>
                    si
                        .CreateButton<Command3>("btn3", "Button3",
                            btn => btn
                                .SetSmallImage(Resources.
                                    _1348119594_preferences_system_16))
                        .CreateButton<Command4>("btn4", "Button4",
                            btn => btn
                                .SetSmallImage(Resources.
                                    _1348119621_internet_web_browser_16)))
                .CreateSeparator()

                .CreateStackedItems(si =>
                    si
                        .CreateButton<Command3>("btn3_1", "Button3",
                            btn => btn
                                .SetImage(Resources
                                    ._1348119708_face_monkey_32))
                        .CreateButton<Command4>("btn4_1", "Button4",
                            btn => btn
                                .SetSmallImage(Resources
                                    ._1348119621_internet_web_browser_16))
                        .CreatePullDownButton("pulldownbutton", "Pulldown",
                            pdb =>
                            {
                                pdb.SetSmallImage(Resources
                                    ._1348119553_face_smile_big_16);
                                pdb.CreateButton<Command3>("btn3_2", "Button3",
                                   btn => btn
                                       .SetSmallImage(Resources._1348119594_preferences_system_16)
                                       .SetLargeImage(Resources._1348119585_preferences_system_32))
                                   .CreateButton<Command4>("btn4_2", "Button4")
                                   .CreateButton<Command1>("btn1_2", "Button1",
                                       btn => btn
                                           .SetContextualHelp(ContextualHelpType.Url, "http://adn-cis.org"))
                               .SetHelpUrl("http://adn-cis.org/forum");
                            })
                )
                .CreatePullDownButton("ADN-CIS_PullDownButton", "ADN-CIS",
                    pdb =>
                    {
                        pdb.SetImage(Resources
                            ._1348119708_face_monkey_32);
                        pdb.CreateButton<Command3>("btn3_1", "Button3",
                            btn => btn
                                .SetSmallImage(Resources._1348119594_preferences_system_16)
                                .SetLargeImage(Resources._1348119585_preferences_system_32))
                            .CreateButton<Command4>("btn4_1", "Button4",
                                btn => btn
                                    .SetSmallImage(Resources._1348119621_internet_web_browser_16)
                                    .SetLargeImage(Resources._1348119621_internet_web_browser_16))
                            .CreateButton<Command1>("btn1_1", "Button1",
                                btn => btn
                                    .SetSmallImage(Resources._1348119553_face_smile_big_16)
                                    .SetLargeImage(Resources._1348119568_face_smile_big_32)
                                    .SetContextualHelp(ContextualHelpType.Url, "http://adn-cis.org"))
                        .SetHelpUrl("http://adn-cis.org/forum");
                    })
                .CreateSplitButton("Test_SplitButton", "split_button",
                    sbd =>
                    {
                        sbd.SetImage(Resources
                            ._1348119708_face_monkey_32);
                        sbd.CreateButton<Command3>("btn6_1", "Split 1",
                            btn => btn
                                .SetSmallImage(Resources._1348119594_preferences_system_16)
                                .SetLargeImage(Resources._1348119585_preferences_system_32))
                            .CreateButton<Command4>("btn6_2", "Split 2",
                                btn => btn
                                    .SetSmallImage(Resources._1348119621_internet_web_browser_16)
                                    .SetLargeImage(Resources._1348119621_internet_web_browser_16))
                            .CreateButton<Command1>("btn6_3", "Split 3",
                                btn => btn
                                    .SetSmallImage(Resources._1348119553_face_smile_big_16)
                                    .SetLargeImage(Resources._1348119568_face_smile_big_32)
                                    .SetContextualHelp(ContextualHelpType.Url, "http://adn-cis.org"))
                        .SetHelpUrl("http://adn-cis.org/forum");
                    });
            ;

            ribbon
                .Tab("MyTab")
                .Panel("Panel2")
                .CreateButton<Command4>("btn4_2", "Button 4");

            ribbon
                .Tab(Autodesk.Revit.UI.Tab.AddIns)
                .Panel("VC1")
                .CreateButton<Command1>("btn1_1", "Button1");

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}