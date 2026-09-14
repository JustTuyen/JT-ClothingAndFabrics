import * as React from 'react';
import icon from '../assets/Icon.png'
//
import './Home/css/Footer.css'
//
import MailOutlinedIcon from '@mui/icons-material/MailOutlined';
import HomeIcon from '@mui/icons-material/Home';
import LocalPhoneIcon from '@mui/icons-material/LocalPhone';
import AlternateEmailIcon from '@mui/icons-material/AlternateEmail';
import ArrowRightIcon from '@mui/icons-material/ArrowRight';
//
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';

//
export default function Footer(){
    const sxId = React.useId();
    return(
        <>
        <footer className="bg-[#FFF1D1] p-4">
            <div className="flex flex-col items-center gap-2 p-4 text-center">
                <p className='font-bold'>ĐĂNG KÝ NHẬN TIN</p>
                <p>Để cập nhật những sản phẩm mới, nhận thông tin ưu đãi đặc biệt và thông tin giảm giá khác.</p>
                <div className="">
                    <Box sx={{ display: 'flex', alignItems: 'flex-end' }}>
                        <MailOutlinedIcon sx={{ color: 'action.active', mr: 1, my: 0.5 }} />
                        <TextField id={`${sxId}-input`} label="Nhập Email@ của bạn" variant="standard" />
                    </Box>
                </div>
            </div>
            <div className="grid grid-cols-1
            md:grid-cols-3  gap-4 p-4">
                <div className="flex flex-col items-start md:items-center gap-2">
                    <div className="flex gap-2 items-center">
                        <ArrowRightIcon/>
                        <p className='font-bold'>Về KingDom</p>
                    </div>
                    <img src={icon} alt='web-icon' className='web-icon'/>
                </div>
              
                <div className="flex flex-col items-start">
                    <div className="flex gap-2 items-center">
                        <ArrowRightIcon/>
                        <p className='font-bold'>LIÊN HỆ HỢP TÁC</p>
                    </div>
                    <List>
                        <ListItem>
                            <ListItemIcon>
                                <HomeIcon sx={{color: 'black'}} />
                            </ListItemIcon>
                            <ListItemText
                                primary="Địa chỉ:"
                                secondary={'24 Vo Thi Sau, Can Tho, VietName'}
                            />
                        </ListItem>
                        <ListItem>
                            <ListItemIcon>
                                <LocalPhoneIcon sx={{color: 'black'}}/>
                            </ListItemIcon>
                            <ListItemText
                                primary="Điện thoại:"
                                secondary={'0916918014'}
                            />
                        </ListItem>
                        <ListItem>
                            <ListItemIcon>
                                <AlternateEmailIcon sx={{color: 'black'}}/>
                            </ListItemIcon>
                            <ListItemText
                                primary="Email:"
                                secondary={'164tuyen@gmail.com'}
                            />
                        </ListItem>
                    </List>
                </div>
                <div className="flex flex-col items-start">
                    <div className="flex gap-2 items-center">
                        <ArrowRightIcon/>
                        <p className='font-bold'>Hỗ trợ - chính sách mua hàng</p>
                    </div>
                    <div className="px-8">
                        <ul className="list-disc">
                            <li>Giới thiệu</li>
                            <li>Liên hệ</li>
                            <li>Phương thức thanh toán</li>
                            <li>Chính sách giao hàng</li>
                            <li>Chính sách đổi hàng</li>
                            <li>Chính sách trả hàng</li>
                            <li>Chính sách bảo mật</li>
                            <li>Chính sách mua hàng</li>
                        </ul>
                    </div>
                </div>
            </div>
        </footer>
        </>
    )
}