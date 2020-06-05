import { ReactComponent as BuildingSvg } from 'assets/images/icon-business.svg';
import { ReactComponent as LandSvg } from 'assets/images/icon-lot.svg';

import React from 'react';
import { CellProps } from 'react-table';
import { formatMoney, formatNumber } from 'utils';
import { IProperty } from '../interfaces';
import { useFormikContext } from 'formik';
import { FastCurrencyInput, FastSelect, Input } from 'components/common/form';
import useCodeLookups from 'hooks/useLookupCodes';
import { FaRegTimesCircle } from 'react-icons/fa';
import _ from 'lodash';

const sumFinancialRows = (properties: IProperty[], key: string): string => {
  const sum = formatNumber(_.reduce(_.map(properties, key), (acc, val) => acc + val) ?? 0);
  return sum === 'NaN' ? '$0' : `$${sum}`;
};

const MoneyCell = ({ cell: { value } }: CellProps<IProperty, number>) => formatMoney(value);

const EditableMoneyCell = (cellInfo: any) => {
  const context = useFormikContext();
  return (
    <FastCurrencyInput
      formikProps={context}
      field={`properties.${cellInfo.row.id}.${cellInfo.column.id}`}
    ></FastCurrencyInput>
  );
};

const EditableClassificationCell = (cellInfo: any) => {
  const classifications = useCodeLookups().getOptionsByType('PropertyClassification');
  const filteredClassifications = classifications.filter(
    (x: any) => x?.label === 'Surplus Active' || x?.label === 'Surplus Encumbered',
  );
  const context = useFormikContext();
  return (
    <FastSelect
      formikProps={context}
      type="number"
      options={filteredClassifications}
      field={`properties.${cellInfo.row.id}.classificationId`}
    ></FastSelect>
  );
};

/**
 * Create a formik input using the passed cellinfo to get the associated data.
 * This information is only editable if this cell belongs to a parcel row.
 * @param cellInfo provided by react table
 */
const EditableParcelInputCell = (cellInfo: any) => {
  if (cellInfo.row.original.propertyTypeId === 1) {
    return cellInfo.value ?? null;
  }
  return <Input field={`properties.${cellInfo.row.id}.${cellInfo.column.id}`}></Input>;
};

export const getColumnsWithRemove = (setProperties: Function, editable?: boolean) => {
  const cols = getColumns(editable);
  cols.unshift({
    Header: '',
    align: 'left',
    accessor: 'id',
    maxWidth: 40,
    Cell: (props: any) => (
      <FaRegTimesCircle
        title="Click to remove from project"
        style={{ cursor: 'pointer' }}
        size={16}
        onClick={() => {
          setProperties(_.difference(_.map(props.rows, 'original'), [props.row.original]));
        }}
      />
    ),
  });
  return cols;
};

export const getColumns = (editable?: boolean): any => [
  {
    Header: 'Agency',
    accessor: 'agencyCode', // accessor is the "key" in the data
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Sub Agency',
    accessor: 'subAgency',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Property Name',
    accessor: 'description',
    maxWidth: 170,
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Classification',
    accessor: 'classification',
    width: 140,
    align: 'left',
    Cell: editable ? EditableClassificationCell : (cellInfo: any) => cellInfo.value,
    clickable: !editable,
  },
  {
    Header: 'Civic Address',
    accessor: 'address',
    align: 'left',
    clickable: true,
  },
  {
    Header: 'Current Zoning Code',
    accessor: 'zoning',
    align: 'left',
    clickable: !editable,
    Cell: editable ? EditableParcelInputCell : (cellInfo: any) => cellInfo.value ?? null,
  },
  {
    Header: 'Potential Zoning Code',
    accessor: 'zoningPotential',
    align: 'left',
    clickable: !editable,
    Cell: editable ? EditableParcelInputCell : (cellInfo: any) => cellInfo.value ?? null,
    Footer: () => <span>Sum</span>,
  },
  {
    Header: 'Netbook Value',
    accessor: 'netBook',
    Cell: editable ? EditableMoneyCell : MoneyCell,
    align: 'left',
    clickable: true,
    Footer: ({ properties }: { properties: IProperty[] }) => (
      <span>{sumFinancialRows(properties, 'netBook')}</span>
    ),
  },
  {
    Header: 'Estimated Value',
    accessor: 'estimated',
    Cell: editable ? EditableMoneyCell : MoneyCell,
    align: 'left',
    clickable: true,
    Footer: ({ properties }: { properties: IProperty[] }) => (
      <span>{sumFinancialRows(properties, 'estimated')}</span>
    ),
  },
  {
    Header: 'Type',
    accessor: 'propertyTypeId',
    width: 60,
    clickable: true,
    Cell: ({ cell: { value } }: CellProps<IProperty, number>) => {
      const icon = value === 0 ? <LandSvg title="Land" /> : <BuildingSvg title="Building" />;
      return icon;
    },
  },
];