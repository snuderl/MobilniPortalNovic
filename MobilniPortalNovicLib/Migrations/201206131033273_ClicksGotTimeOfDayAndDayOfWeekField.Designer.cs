// <auto-generated />
namespace MobilniPortalNovicLib.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    
    public sealed partial class ClicksGotTimeOfDayAndDayOfWeekField : IMigrationMetadata
    {
        string IMigrationMetadata.Id
        {
            get { return "201206131033273_ClicksGotTimeOfDayAndDayOfWeekField"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return "H4sIAAAAAAAEAN1d227kNhJ9D7D/IOhpN0C62w4QTAbtCWbtcWBkfMG0Z/bRoCV2mxhdOhLbsb8tD/tJ+wtL6sq7KEp980swTZHFYvFUsYqscv7393/nv73EkfcMsxylyZl/Mpn5HkyCNETJ6szf4OVP7/zfPvzjh/mnMH7xvtX9Tmk/MjLJz/wnjNfvp9M8eIIxyCcxCrI0T5d4EqTxFITp9HQ2ezc9mU0hIeETWp43/7JJMIph8YP8PE+TAK7xBkTXaQijvGonXxYFVe8GxDBfgwCe+dfpI4oSdJdmGEQ36TMKPqPHSTnO9z5GCBCeFjBa9mRw9itl0G+mJpN/Ikzi1/vXNSwYOPNv4F/5AmHI9iL9/oCvXANpusvSNczw6xe4rMbScVeh7035sVNxcDNUGEfZOPOvEvzzqe/dbKIIPEakYQmiHPre+pf3C5xm8HeYwAxgGN4BjGFGtuoqhMUyKnG8X/9iJ5Ffp7NTKpEpSJIUA0z2XWJeYJX+t2Z0gTMCId+7RC8w/AyTFX5qmL0GL3UL+afvfU0QQRwZhLMNZBdX/hYmvQHPaFXwI0x/CWFIIPAFRsXX/AmtSyRM6JeHZvO8yyyNv6RRNaT58HAPshXEZAWp6usi3WSBwM582mLEiBxKygU1dJwLaupxR4CaTRaNCpqqv3lSKp/dwFXUkQpO3ZtjpvMZ5PjrOqSbVhO6ID/uUWwhDK0KtTrirkW1nqi1qNYxJy06JytcpdmriybVY120iR17BBq1F2DfgYws0EJS/Yw6T1aJy/rjw/kTikLSncWm9FGy8nIPlaU38djOO5w7UXv0/DtpENXDSxQ5eTB0rIv21OOOQHMWT8StJN4oJjPu/lC6RzgaV3Gtpt3beu82j/TM6n9+OTk7HUKwNvEd5zJKvu9AknpbZLKUtfY/tL1aWyR9lGyR3ENli7pcdDNnZQ8FV4UboeWo+DrMt4hQ8P08JYEpzJz8CzreybmoBx6BfSx4HUVnP6dBNeMeHPChSv41h9lQGhfg9Xb5Hwi/DyNDhX+7JMT2ZP6MoUThaSidIkbZHtqejGOk6iA7R8pefY0S3c1uLsteGg7pRzN3RY++nJndXpa8yqArO5i5NBp2a1NaiMrBhNZq1deC2qnjQRhQymoydnim0EzdXn3M8zRABavMkfygudj8lISeMcQv11BfDhCWNxFGa4IoMvWZ/6MkDR3BJiZrCbYXDTzRE18E0m1yASOIofcxwMXt9DnIAxDKtorIJORbCPYgjS8RiIgXnOMMoATLQEVJgNYgMrEuDOpxBUwZa6YQv1zANUwoPE37YDM3e+8kz99MIwisSz7zKQMoM87kOFgHDUNQ3MKjNXg8PGaTyYkBdobrgG7aMp6d8KNdn80+6q+xeuFIKwcbHuSrnr0gSo5mdLtuCG14g1O6IfZWTB8RWcBp/5ZMy/4Okajdm+E87BKDigceBUz4IHYk7HGxb8eRfECYY9m22Wv1U5gb1th9GDb3Lk5OZbCkPeHMkRNjltgLjx64MwddFqDePwaNS7D1poaewKZ9GsbDrjGpiPc0cOHj5JGxyIXYDPEycj9oDLKs2+y9OlJ2xx+7L8Pm3zX2On2/jluQkTF4nH6gcQm7jEpMe3WA/mB5x0Lfz8gImFVMyBlqxQvbCz45lVBKxy8gFoL03Pfa6xvpPkRCIk+kSoOSCJTuYMfgSoBIyUIL4A4q9XGmW0fpFXSxQsGgZINT1A4q1FKpiJSHgjCY2WZenkzaC9NHmRcjoq7rDq1huNk5Cbhdt2YMCQZBoqngF2excEXmgrz4jmsdy4sdZgUsAg2S0F/kWNFyEIfi8VQWR8edhOWthLChlSoZpKG/h9i6NEqzYpCEHBlbxMbuEuCi4S7lctEK9dOVQjO6w7YegRu7j5VhNGmHMUazEe5QyZT2tUMqcuBgGToMkAYXJTB0qoNiXCmYTIWFG9vDkR0gkdFtRv341DhGzbf5tEzorxrmU03m//warNcoWTGVAFWLtyjLAM5/WvRP8I9LGtMgV+T5N9w2M+E0AysofKX5UiG8RFmOLwAGj4A+vZ2HsdTNwQ2sZxa9QXkja2+mHkH/rfM+2/qIic5LaeV7SZYcU4+8eCtVeBbyUI+WaYAIZJrKhfM02sSJ+QlMT6VMZmVplC0yhflUWIMUAEhyk6IpfiOstqk8WEbbItVxabE96mE6odb3maxYdXeceipF5j5LomiwH98m4Yt86LZYCxLmdZODiuHVU0+Ny61nyXEfDgaAjI0eDYW688gCifqhOnmzUTsrblM0b8CCtcHQUZDfN1lq3a+fe0NC682NemKoHFfLE0M91KTGKhXuBwA+iZs7fbgv9hSrxGyWVNVkT0PJkAMvTdI0h8m6sZ/xHX4EjKu6ZfI0Z3GLloNRsMq1Hs/MapOOC7qdptY4XLtndcIxt2Hq9OVOOjIWmeYeO9+kA3O737T2cwWG25D6gYOlonv00FNhcnxZQkxzHxvUJPrydqhp3rbW7knnysB8NJVT3TlYqJp62Hbh0yaLinSSfYRAUlgvdmlmb8J7IYyfVyF1d5W/FGOXXXyPiOgZhTS+XrzmGMYT2mGy+DMiZqc4SesO1yBBS5jj+/Q7pH+xYDZ75/7nAZpM3zwPo0P/GwGIiqEz1blngcANk7ScPIMseALZP2Pw8i9z/cWRFMhvRWRMYbtWYk7F6j3kr9hIqQC9WPzw8nP6A3eXAh1hsfchKZRMSFd4LWzrAN3cf9HwVnZAVew7TFG5At5hpEblSyi0tdRUK6M5uM7MyfowRbZ9JHN0xaBbwb1UxOmMB7GIc/DBNAwWfB2YCwWpKNOFiFSSuStFOZICva2AWiysG8MsuBbLtclCu65n4zOFHAr0nFP6Na+KWyuCe6tFb2MWuZVh7u4L2/ZRyFas1Wbeoyte64WMLVmTNs/JqWjpaNBkeO3ae7Kxa/GZ1UnwhnBjfTqMWFD21orIeu3blrDDp3k7F2oMKlLcRzHYWyr+KrNAd1mLdRj40Twybb2Q680Vbx2A77NPHO3Fo+6Lp4MoupKTcsX9VJdaGSutykfHMz98TMnOl/cH+gobcYrSz5LIl80q0uoSBakuqoG/XBnVfFKRN+Ryq6RTHvVK6ZSfdNJR1xNIy+C0R14K91m5HKZH93TlwSNNUzaryKvrAbZbISaCkk+87ioMk8pdDrkGTFVloMhh0d5qWIw+hBov9+WKKsinVB5QAZcV9Ha6tK2VaPVi3OBE6FL4DrAGi7WbbSLUwS111EIrdxM18tJ7FFLJmVXExWL+LyvEzcvRqiVB/58rCQw456rpc5Us09rPEziquwjPL9cQg5B4Xh8zjJYgwORzAPO8+NuG30C0IV0+xY8wvEpuN3i9wWTJMH6MOHFSX9E0f1EtxvM8v10XfxVzjCUQNhF9Cb1N/r0hx1jD96XiwUhDgjqh1ZsW3UtM37ZWrw2lmzSxJFSJr/Gd72G8jgix/DZZgGeo561bhrzE5hcIrDIQsxIsW+rnGkBmZqYgE7Aj2vnITwLXMH758H8PB75hV2gAAA=="; }
        }
    }
}