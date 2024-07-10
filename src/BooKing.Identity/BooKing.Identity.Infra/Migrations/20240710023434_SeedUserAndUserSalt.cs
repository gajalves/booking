using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooKing.Identity.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedUserAndUserSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert data into [Identity].[User]
            migrationBuilder.Sql(@"
                INSERT INTO [Identity].[User] (Id, Email, Name, Password) VALUES 
                ('7FE95E81-98F0-4D54-B723-0634E067F7BE', 'user1@mail.com', 'Emily Johnson', 'd1UsHOJyDsbtjluLFPedC4i+MB2RLsMMQ0aHJeFjWjN7V0F/PET/pVg3ubNsf4DF2S6uwR7/cAuxYzJKuv9J7rgg+Gp/uUq1Q4bas/fi+DEH7xdtYo81z+3cF7W8lz9A+YCRjeHY0Z4BOp65OfU/XmzCwX0YmXuzmaIQy3D3jpzodT3Gy0PggIsI/105vcBBADD2NFvgS3TuNmvaFgbtl2dysAsaILJfJhxFEK/JpDlc0DVQOezRe/NnH9mwJ2pXQ/QuOGTgCvnEdVJyxZ1ICfrz1wXxBiyI/UTDpbweO1JZOIFhjPiCbkNscf04eCg2a0L7zmcnOJbUQiRxfl/yxg=='),
                ('1866E30D-9813-462A-B307-4A442A8FBF74', 'user2@mail.com', 'Daniel Martinez', '1hRFaO2y23i/JMpQb/ssM0OW+wCTW39tK41LiX16CIu6+s6E06mIKgFbUTllN5sITTlZMFg9y3Gly+VtKiBXe7rKulWiFntThW4u5YChQIg0Ohe7OOL8Vfkph/Hr7wR/DDKfku2YU7vqoY6tKI6we6Axv+6d6HoM8I3gG+5vBsQwVrZoYLCcGQWwijelX0SnJFxGYDcJ1s9FWyYIwRHj+Ht7CbHcudEIb5gXP/f4NIDlffcOAbPwtNOicwal+8DjfPfw7OqblKtQCOQr+KVdRM7Mj3JI0dW7PqS4Gglm6JY2Ps+C0ldaTn9NwycblRlr/1y3oRINOFK1crmAsgr/fA=='),
                ('0CFCABA1-5B76-4B9A-9B37-BA3A8F6BC4AA', 'user5@mail.com', 'Sophia Brown', '/BRHr663RGYEXcnfYFuKLf4oGVWGpnuk4eU7j6+jcyF3y+2mDsAgJArfsWzMX6xgre8F8ghTVe2YcIL0UCEeyLEitfixoUBoH3YGSRJTyQRvjWP2fZfJ/Ez+Yy6fltwcHOE+FHuGRZWxQPfTRHdwejmP33+Ebj3PcX7Xm8CJOkZD5QDBXN60X7ox/QL1+QrfTaXGSY9j0sADAJLzjMCavdy0esEcAP8LcTn+pLBEbPa6N0MkXaaiQkyGiiKOszOew7sys0WVXyJiMA5xtFFHywOs4c7L6KNmmU8t9p0ERY6zE8yT42t1xPSjqOKICpQAy5BS6Gy412CfgTDeTbkR5Q=='),
                ('6FE6E56E-9B23-42BA-9525-CC0C8C5AC768', 'user3@mail.com', 'Liam Thompson', 'MDCT8/9QVq4yjVVBpVpXQCesGDtu2CfozHmc/FHTAiJwdV2WgHcQWaJxbuSNYXNpiEstnINQS1Y9cnwAJx7Gc+fz/CdxGgBF+k/UqF2GqwZzDRJUBnH1VywFTo+aKe1z2dPoXYsBb36NnufXKfZWGbLtPlT8U4rOJ4ykBmVzP+aGZiGgFodb+ecf98I7q1p9PeoNclv5RXPxpzAAJwQGRd9qbiZhLDTtYx/ySvhBMajnA7IpHXJoHHhYDqz6at7yK/mUYBaaP2i6/f+nPEPCfUXCkFmOO99Ec250QK5Ajtpjj4u0lEfF3UxAp7ubYDZC0AlYDJrpcvSe3WQQ5aIqMg=='),
                ('0669483C-AEEF-49EC-98CF-D1DE64BD7C50', 'user4@mail.com', 'Olivia Rodriguez', 'DoOLNNMiLLHuxqtxTY/F6JduAmeJhsbJWxWG72w2Pa8gBt0RVosqLLFEyR4ANWQgtqJ9X1pd4JYpdcZ/Rz/rJQ8lvTaNXoPXZKooxvN0zopi/tQwU8ZCsLjF34aAWcCnoBnsw+iHHnTa14m6XMhQHZGlgNTe+7pM4lZBiJ/lCXocekEe4u5ENcAayf3SZ4NoBNX3wQn9Ys9H2SXn3oXBRfv18F0L72CBnNaW3Tt7CICH1sbTkHmtSlPfz6AjoLuYTwauaZheI9SUVGeIicIkANkmvXbRLyNoezeAA8p5ok4iYMQMv9f3hI4dWB6WejuI4j+yWH68fJNfXcqCw65cMg==');
            ");

            // Insert data into [Identity].[UserSalt]
            migrationBuilder.Sql(@"
                INSERT INTO [Identity].[UserSalt] (Id, UserId, Salt) VALUES 
                ('52ADB916-0739-4370-A450-11699D2EAD9A', '7FE95E81-98F0-4D54-B723-0634E067F7BE', 'gE2Q2RLuf2AKooR4su71D/CVRVc906gzV1kgcYEo41M='),
                ('5F84501B-2C76-447F-BC13-4B58423014F6', '1866E30D-9813-462A-B307-4A442A8FBF74', 'OlBwJnvbLr464Bi58vxJiDNfgKKrFV0F84nAHXoNDLI='),
                ('63087A2B-4AA7-4E77-8D3F-6570240E1426', '6FE6E56E-9B23-42BA-9525-CC0C8C5AC768', 'tdc/hM65wDef/husYzcfMiYb/tqjy047WCnZZ5yU5Cc='),
                ('6E9E1184-1AAF-4A7D-B18C-BF68283EA131', '0CFCABA1-5B76-4B9A-9B37-BA3A8F6BC4AA', '5ItLJ+ikSAl2t+nifK5qp1tUuYi3MJba1iyC02grbn8='),
                ('3063FA6F-930C-4272-8D18-E5A7E4847DAE', '0669483C-AEEF-49EC-98CF-D1DE64BD7C50', 'OQ4opBaSN1DojT6/syCpWn7G7RF/zb0c3KPULKcIQww=');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
