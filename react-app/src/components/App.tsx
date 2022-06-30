import React from 'react';
import '../css/components/App.scss';
import { Menu } from './Menu';
import { Main } from './Main/Main';
import { Party } from './Party/PartyMain';
import { FilterContextProvider } from './FilterContext';
import { ClientContext, ClientContextProvider } from './ClientProvider/ClientContextProvider';

const App = () => {
  return (
    <div className='app-component'>
      <ClientContextProvider>
        <FilterContextProvider>
          <Menu/>
          <Party/>
          <Main/>
        </FilterContextProvider>
      </ClientContextProvider>
    </div>
  );
}

export default App;
