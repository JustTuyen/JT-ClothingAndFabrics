import Comment from '../../components/Item/Comments'
import SuggestBox from '../../components/Item/suggestion';
import ItemGallery from '../../components/Item/ItemGallery'
import Footer from "../../components/Footer";
import Navbar from "../../components/NavBar";
import ItemTab from '../../components/Item/ItemTab';
//
import * as React from 'react';
import Box from '@mui/material/Box';
import NumberSpinner from '../../components/NumberSpinner';
import Button from '@mui/material/Button';
import Stack from '@mui/material/Stack';

//
export default function ItemPage() {
    const [selectedSize, setSelectedSize] = React.useState("m");
    const options = [
        { label: "Small", value: "s" },
        { label: "Medium", value: "m" },
        { label: "Large", value: "l" },
        { label: "Small", value: "s" },
        { label: "Medium", value: "m" },
        { label: "Large", value: "l" },
        { label: "Small", value: "s" },
         { label: "Small", value: "s" },
        { label: "Medium", value: "m" },
        { label: "Large", value: "l" },
        { label: "Small", value: "s" },
    ];

    return (
    <>
    <Navbar/>
    <div className="min-h-screen">
        <div className="flex gap-2 p-2 bg-[#EEEEEE]">
            <p>Trang chủ</p>
            <p>/ Sản phẩm mới</p>
            <p>/ Quần Jeans Slim Maxlook 7473</p>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-2 p-2">
            <div className="">
                <ItemGallery/>
            </div>
            <div className="shadow-md p-4 flex flex-col gap-2">
                <div className="p-4">
                    <div className="flex max-w-full">
                        <p className="text-[24px] font-bold">
                            Quần Jeans Slim Maxlook 7473: Định Hình Phong Cách Nam Tính, Lịch Lãm Cùng Old Sailor
                        </p>
                    </div>

                    {/* STATUS */}
                    <div className="flex flex-row gap-4">
                        <p className="text-sm text-black">Tình trạng: <span className='font-bold'>Còn hàng</span></p>
                        <p>|</p>
                        <p className="text-sm text-black">Thương hiệu: <span className='font-bold'>Old Sailor</span></p>
                    </div>
                </div>
                
                {/* GIA SAN PHAM */}
                <div className="text-[24px] font-bold p-4 rounded-2xl
                text-[#DF301C] bg-[#EEEEEE]">
                    <p>540,000₫</p>
                </div>

                {/* KICH THUOC */}
                <div className="grid grid-cols-4 gap-2 p-4">
                    <div className='col-span-1 flex items-center'>
                        <p className="font-black text-sm text-black">Kích thước:</p>
                    </div>

                    <div className="col-span-3 flex flex-wrap gap-2 justify-start">
                        {options.map((option) => (
                        <label key={option.value} className="cursor-pointer">
                            <input
                            type="radio"
                            name="size"
                            value={option.value}
                            checked={selectedSize === option.value}
                            onChange={(e) => setSelectedSize(e.target.value)}
                            className="sr-only peer"
                            />
                            <span
                            className="inline-block px-4 py-2 text-sm font-medium border rounded-lg 
                            transition-all duration-200 border-gray-300 bg-white text-black
                            hover:border-[#DF301C] hover:text-[#DF301C]
                            peer-checked:bg-[#DF301C] peer-checked:text-white peer-checked:border-[#DF301C]
                            peer-focus-visible:ring-2 peer-focus-visible:ring-[#DF301C] peer-focus-visible:ring-offset-2"
                            >
                            {option.label}
                            </span>
                        </label>
                        ))}
                    </div>
                </div>

                {/* SO LUONG */}
                <div className="grid grid-cols-4 gap-2 p-4">
                    <div className='col-span-1 flex items-center'>
                        <p className="font-black text-sm text-black">số lượng:</p>
                    </div>
                    <div className="">
                        <Box
                        sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 4,
                        width: 150,
                        justifyContent: 'start',
                        }}
                        >
                            <NumberSpinner size="small" />
                        </Box>
                    </div>
                </div>

                {/* ACTIONS */}
                <div className="p-4">
                    <Stack spacing={2} direction="row">
                        <Button size='large' fullWidth 
                        variant="contained" 
                        sx={{backgroundColor: '#FF9100', fontWeight: 'bold'}}
                        >THÊM VÀO GIỎ</Button>
                        <Button size='large' 
                        fullWidth variant="contained"
                        sx={{backgroundColor: '#DF301C',fontWeight: 'bold'}}
                        >MUA NGAY</Button>
                    </Stack>
                </div>
            </div>
        </div>
        <div className="p-2">
            <ItemTab/>
        </div>
        <div className="p-2 border-t border-[#EFEFEF] mt-4">
            <Comment/>
        </div>
        <div className="p-2 border-t border-[#EFEFEF] mt-4">
            <SuggestBox/>
        </div>
    </div>
    <Footer/>
    </>
    );
}