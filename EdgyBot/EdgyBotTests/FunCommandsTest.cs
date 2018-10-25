using System;
using EdgyBot.Modules.Categories;
using Xunit;

namespace EdgyBotTests
{
    public class FunCommandsTest
    {
        [Fact]
        public void ConvertToEmoji_AddSpaceBetweenLetters()
        {
            var module = new FunCommands();
            var actual = module.ConvertToEmoji("kil");
            var expected = ":regional_indicator_k: " +
                ":regional_indicator_i: " +
                ":regional_indicator_l:";

            Assert.Equal(expected, actual);
        }
    }
}
