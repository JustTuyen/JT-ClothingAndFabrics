
import React from "react";
//
import Sizing from '../../assets/sizing.webp'

//

import PlayArrowIcon from '@mui/icons-material/PlayArrow';
//

//css
import './Css/ItemTab.css'

function Descriptions(){
    return(
        <>
        <div className="p-4 flex flex-col gap-4">
            <div className="flex justify-center">
                <img src={Sizing} alt="Size table" className="sizing-img"/>
            </div>
            <div className="flex flex-col gap-4">
                <div className="w-full">
                    <h1 className="text-xl sm:text-2xl md:text-[28px] lg:text-[32px] font-bold text-black leading-tight tracking-tight">
                        Quần Jeans Slim Maxlook 7473: Định Hình Phong Cách Nam Tính, Lịch Lãm Cùng Old Sailor
                    </h1>
                </div>
                <div className="w-full">
                    <div className="flex gap-2 items-center">
                        <PlayArrowIcon/>
                        <p className="font-bold text-lg">MÔ TẢ:</p>
                    </div>
                    <p className="text-black text-md leading-tight tracking-tight">Trong tủ đồ của một quý ông hiện đại, chắc chắn không thể thiếu những chiếc quần jeans vừa vặn, tôn dáng và mang lại sự thoải mái tối đa. Hiểu được điều đó, Old Sailor mang đến siêu phẩm Quần Jeans Slim Maxlook (Mã: 7471-7473) – sự kết hợp hoàn hảo giữa chất liệu denim cao cấp và thiết kế slim fit trẻ trung, giúp phái mạnh tự tin khẳng định phong cách cá nhân.</p>
                </div>
            </div>
        </div>
        </>
    )
}

function ReturnPolicy(){
     return(
        <>
        <div className="p-4 flex flex-col gap-4">
            <div className="w-full">
                <p className="font-bold">1. Lưu Ý khi nhận hàng:</p>
                <p className="p-2">- Khách hàng có thể kiểm tra hàng trước khi nhận</p>
                <p className="p-2">- Nếu không vừa ý với sản phẩm khách hàng có thể từ chối nhận. Nếu ưng ý, Khách nhận hàng và thanh toán cho shipper.</p>
                <p className="p-2">- Trường hợp hàng đã nhận khách hàng vẫn có thể ĐỔI trong 14 ngày (sản phẩm phải còn nguyên tem mác & hoá đơn)</p>
            </div>
            <div className="w-full">
                <p className="font-bold">2. Quy định về phí đổi / trả hàng:</p>
                <p className="p-2 underline">Đối với khách hàng thanh toán Cod:</p>
                <p className="p-2">* Tất cả đơn hàng khách hàng sẽ chịu phí ship 2 chiều (phí ship gửi đi và phí đổi hàng)</p>
                <p className="p-2">*Trường hợp đổi hàng do lỗi sai do hàng hóa lỗi, gửi nhầm size, nhầm màu… phí ship tính vào chi phí của KingDom</p>
                <p className="p-2 underline">Đối với khách hàng thanh toán Cod:</p>
                <p className="p-2">Tư vấn viên sẽ hướng dẫn khách hàng các bước cần thiết để tiến hành trả đổi trả.</p>
                <p className="p-2">Khách hàng được hỗ trợ đổi hàng với trường hợp mẫu mã không vừa hoặc không ưng. Khách hàng đổi trực tiếp tại hệ thống cửa hàng OLD SAILOR trên toàn quốc hoặc có thể liên hệ online để đổi hàng . Hàng hóa khi đổi cần được giữ nguyên tem mác và chưa qua sử dụng, giặt tẩy.</p>
                <p className="p-2">- Sản phẩm SALE không trả hàng</p>
                <p className="p-2">- Sản phẩm được tặng shop ko hỗ trợ đổi và trả</p>

            </div>
        </div>
        </>
    )
}

function DeliveryPolicy(){
     return(
        <>
        <div className="p-4 flex flex-col gap-4">
            <div className="w-full">
                <p className="font-bold">Khách nhận hàng tại Store. (Khách mua hàng trực tiếp tại các store)</p>
            </div>
            <div className="w-full">
                <p className="font-bold">Giao hàng tận nơi (Khách được chọn khi đặt hàng tại website hoặc trên Facebook):</p>
                <p className="p-2">– Nội thành TPHCM: Dự kiến trong 24h kể từ khi đơn hàng được xác nhận.</p>
                <p className="p-2">– Tuyến tỉnh: Dự kiến trong 3-4 ngày kể từ khi đơn hàng được xác nhận.</p>
                <p className="p-2 underline">Lưu ý:</p>
                <p className="p-2">- OLD SAILOR sẽ giao hàng trong giờ hành chính (8h- 17h), nếu có bất kỳ yêu cầu nhận hàng nào ngoài thời gian này quý khách vui lòng gửi yêu cầu cho OLD SAILOR tại thời điểm mua hàng..</p>
                <p className="p-2">- Trường hợp không đáp ứng được thời gian dự kiến trên do các lý do khách quan/chủ quan OLD SAILOR sẽ liên hệ đến quý khách hàng để thỏa thuận lại.</p>
                <p className="p-2">- Trong trường hợp giao hàng chậm trễ mà không báo trước, quý khách có thể từ chối nhận hàng và chúng tôi sẽ hoàn trả toàn bộ số tiền mà quý khách trả trước (nếu có) trong vòng 7 – 10 ngày.</p>
                <p className="p-2">- Công ty cam kết tất cả hàng hóa gởi đến quý khách đều là hàng chính hãng mới 100%. Những rủi ro phát sinh trong quá trình vận chuyển có thể ảnh hưởng đến hàng hóa, vì thế xin Quý Khách vui lòng kiểm tra hàng hóa thật kỹ trước khi ký nhận. OLD SAILOR sẽ không chịu trách nhiệm với những sai lệch hình thức của hàng hoá sau khi Quý khách đã ký nhận hàng.</p>

            </div>
        </div>
        </>
    )
}


export default function ItemTab(){
    
    const [activeTab, setActiveTab] = React.useState('description');
    const renderSubView = () => {
        switch(activeTab){
            case 'description':
                return <Descriptions/>
            case 'returnPolicy':
                return <ReturnPolicy/>
            case 'deliveryPolicy':
                return <DeliveryPolicy/>
        }
    }

    return(
        <>
        <div className="p-4">
            <div className="flex justify-start gap-4 text-lg
            md:justify-center md:text-3xl md:gap-8
            ">
                <span
                onClick={() => setActiveTab('description')}
                className={`hover-line1 tab ${
                    activeTab === 'description'
                    ? 'font-extrabold'
                    : 'font-light'
                }`}>
                    MÔ TẢ SẢN PHẨM
                </span>
                
                <span
                onClick={() => setActiveTab('returnPolicy')}
                className={`hover-line2 tab ${
                    activeTab === 'returnPolicy'
                    ? 'font-bold'
                    : 'font-light'
                }`}>
                    CHÍCH SÁCH TRẢ HÀNG
                </span>
                <span
                onClick={() => setActiveTab('deliveryPolicy')}
                className={`hover-line3 tab ${
                    activeTab === 'deliveryPolicy'
                    ? 'font-bold'
                    : 'font-light'
                }`}>
                    CHÍCH SÁCH GIAO HÀNG
                </span>
            </div>
            <div className="border-t border-[#EFEFEF] mt-4">
                {renderSubView()}
            </div>
        </div>
        </>
    )
}