import Icon from '../assets/Icon.png'
import './NavBar.css'
import * as React from 'react';
import PermIdentityIcon from '@mui/icons-material/PermIdentity';
import ShoppingBasketOutlinedIcon from '@mui/icons-material/ShoppingBasketOutlined';
import SearchIcon from '@mui/icons-material/Search';
import Stack from '@mui/material/Stack';
import IconButton from '@mui/material/IconButton';
import Tooltip from '@mui/material/Tooltip';
import Badge, { badgeClasses } from '@mui/material/Badge';
import { styled } from '@mui/material/styles';
import Button from '@mui/material/Button';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
//
import LongMenu from './LongMenu';
const CartBadge = styled(Badge)`
  & .${badgeClasses.badge} {
    top: -12px;
    right: -6px;
  }
`;


export default function Navbar(){
    const id = React.useId();
    const buttonId = `${id}-button`;
    const menuId = `${id}-menu`;
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };


    const buttonId1 = `${id}-button`;
    const menuId1 = `${id}-menu`;
    const [anchorEl1, setAnchorEl1] = React.useState<null | HTMLElement>(null);
    const open1 = Boolean(anchorEl1);
    const handleClick1 = (event: React.MouseEvent<HTMLButtonElement>) => {
        setAnchorEl1(event.currentTarget);
    };
    const handleClose1 = () => {
        setAnchorEl1(null);
    };

    return(
        <>

        <nav className="w-full sticky top-0 z-50 p-4">
            <div className="grid grid-cols-3 md:grid-cols-5
            gap-4 items-center">
                <div className="md:hidden flex">
                    <LongMenu/>
                </div>
                <div className="col-span-1 shrink-0 flex justify-center items-center">
                    <img src={Icon} alt="Web-icon" className="object-contain" />
                </div>

                <div className="col-span-3 flex-wrap 
                items-center gap-4 justify-center hidden md:flex
                ">
                    <Button sx={{ color: 'white', fontWeight: 'bold', borderRadius: '16px' }}>
                        Category #1
                    </Button>

                    <Button sx={{ color: 'white', fontWeight: 'bold', borderRadius: '16px' }}>
                        Category #2
                    </Button>  
                    <Button sx={{ color: 'white', fontWeight: 'bold', borderRadius: '16px' }}>
                        Category #2
                    </Button>
                    <Button sx={{ color: 'white', fontWeight: 'bold', borderRadius: '16px' }}>
                        Category #2
                    </Button>
                    <Button sx={{ color: 'white', fontWeight: 'bold', borderRadius: '16px' }}
                    id={buttonId1}
                    aria-controls={open1 ? menuId1 : undefined}
                    aria-haspopup="true"
                    aria-expanded={open1}
                    onClick={handleClick1}>
                        Category #2
                    </Button>
                    <Menu
                        id={menuId1}
                        anchorEl={anchorEl1}
                        open={open1}
                        onClose={handleClose1}
                        slotProps={{
                        list: {
                            'aria-labelledby': buttonId1,
                        },
                    }}
                    >
                        <MenuItem onClick={handleClose1}>SubCategory</MenuItem>
                        <MenuItem onClick={handleClose1}>SubCategory</MenuItem>
                        <MenuItem onClick={handleClose1}>SubCategory</MenuItem>
                    </Menu>
                    
                </div>

                <div className="col-span-1 flex items-center justify-end">
                <Stack direction="row" spacing={1}>
                        <Tooltip title="Tìm kiếm">
                        <IconButton sx={{ color: 'white' }}>
                            <SearchIcon />
                        </IconButton>
                        </Tooltip>

                        <Tooltip title="Xem giỏ hàng">
                            <IconButton sx={{ color: 'white' }}>               
                                <ShoppingBasketOutlinedIcon />            
                                <CartBadge badgeContent={2} color="error" overlap="circular" />
                            </IconButton>
                        </Tooltip>

                        <Tooltip title="Tài khoản">
                            <IconButton sx={{ color: 'white' }}
                                id={buttonId}
                                aria-controls={open ? menuId : undefined}
                                aria-haspopup="true"
                                aria-expanded={open}
                                onClick={handleClick}
                            >
                                <PermIdentityIcon />
                            </IconButton>   
                        </Tooltip>
                        <Menu
                            id={menuId}
                            anchorEl={anchorEl}
                            open={open}
                            onClose={handleClose}
                            slotProps={{
                            list: {
                                'aria-labelledby': buttonId,
                            },
                            }}
                            >
                            <MenuItem onClick={handleClose}>Profile</MenuItem>
                            <MenuItem onClick={handleClose}>My account</MenuItem>
                            <MenuItem onClick={handleClose}>Logout</MenuItem>
                        </Menu>
                    </Stack>
                </div>
            </div>
        </nav>
        </>
    )
} 